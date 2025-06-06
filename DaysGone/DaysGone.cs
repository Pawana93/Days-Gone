﻿using Days_Gone.Config;
using Days_Gone.Helper;
using Days_Gone.Interfaces;
using Days_Gone.Services;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Days_Gone;

public class DaysGone : Mod
{
    /// <summary>The total amount of days played</summary>
    private int totalDays;
    /// <summary>The config for the mod</summary>
    private DaysGoneConfig config;
    /// <summary>The service to calculate the days</summary>
    private readonly DaysCalculationService _calculationService = new();

    /**
     * <summary>
     * The Entry Point of the Mod
     * </summary>
     * <param name="helper">The Helper for the Mod</param>
     **/
    public override void Entry(IModHelper helper)
    {
        // Load the config
        this.config = this.Helper.ReadConfig<DaysGoneConfig>();

        helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        helper.Events.GameLoop.DayStarted += OnDayStarted;
        helper.Events.Display.RenderingHud += OnRenderingHud;

        // GMCM-Registration
        helper.Events.GameLoop.GameLaunched += OnGameLaunched;
    }

    /// <summary>The Code that will be run, when a save file is loaded</summary>
    private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
    {
        // Calculate current day count
        totalDays = _calculationService.CalculateTotalDays();
    }

    /// <summary>The Code that will be run, when a day starts</summary>
    private void OnDayStarted(object sender, DayStartedEventArgs e)
    {
        // Calculate current day count
        totalDays = _calculationService.CalculateTotalDays();
    }

    /// <summary>The Code that will be run, when the HUD is rendered</summary>
    private void OnRenderingHud(object sender, RenderingHudEventArgs e)
    {
        // Check if the World is ready to run
        if (!Context.IsWorldReady) return;

        // Format the Text to show
        string text = $"{this.Helper.Translation.Get("dayCounter.text")} {totalDays}";

        // Show additional Text, if the option is set
        if(config.ShowSeasonYear) text += $"\n{Game1.currentSeason} Y{Game1.year}";

        // Position the Text and Color
        Vector2 position = new(config.PositionX, config.PositionY);
        Color color = GetColorFromString(config.TextColor);

        // Draw the Text in the UI
        Game1.spriteBatch.DrawString(Game1.smallFont, text, position, color);
    }

    /// <summary>The Code that will be run, when the game is launched</summary>
    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        // Check if GenericModConfigMenu is installed and loaded
        if (!this.Helper.ModRegistry.IsLoaded("spacechase0.GenericModConfigMenu")) return;

        try
        {
            // Get the API for the GenericModConfigMenu
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu == null) return;

            // Register the Mod in the Config Menu
            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.config = new DaysGoneConfig(),
                save: () => this.Helper.WriteConfig(this.config),
                titleScreenOnly: false
            );

            // Add the Options to the Config Menu
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.xPosition.name"),
                tooltip: () => this.Helper.Translation.Get("config.xPosition.tooltip"),
                getValue: () => this.config.PositionX,
                setValue: value => this.config.PositionX = value,
                min: 0,
                max: Game1.viewport.Width
            );

            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.yPosition.name"),
                tooltip: () => this.Helper.Translation.Get("config.yPosition.tooltip"),
                getValue: () => this.config.PositionY,
                setValue: value => this.config.PositionY = value,
                min: 0,
                max: Game1.viewport.Height
            );

            configMenu.AddTextOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.textColor.name"),
                tooltip: () => this.Helper.Translation.Get("config.textColor.tooltip"),
                getValue: () => this.Helper.Translation.Get(this.config.TextColor.ToLower()),
                setValue: value => this.config.TextColor = value,
                allowedValues: ColorHelper.GetAllowedColors(),
                formatAllowedValue: colorKey => this.Helper.Translation.Get(colorKey.ToLower())
            );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("config.showSeasonYear.name"),
                tooltip: () => this.Helper.Translation.Get("config.showSeasonYear.tooltip"),
                getValue: () => this.config.ShowSeasonYear,
                setValue: value => this.config.ShowSeasonYear = value
            );
        }
        catch(Exception ex)
        {
            // If there is an error, we log it to the SMAPI Console
            this.Monitor.Log($"GMCM integration failed: {ex}", LogLevel.Warn);
        }
    }

    /**
     * <summary>
     * Get the Color from the String
     * </summary>
     * <param name="colorName">The Name of the Color</param>
     * <returns>The Color</returns>
     **/
    private Color GetColorFromString(string colorName)
    {
        return colorName.ToLower() switch
        {
            "red" => Color.Red,
            "blue" => Color.Blue,
            "green" => Color.Green,
            "black" => Color.Black,
            "white" => Color.White,
            "yellow" => Color.Yellow,
            _ => Color.White
        };
    }
}
