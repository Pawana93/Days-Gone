using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks.Sources;
using Days_Gone.Interfaces;
using Days_Gone.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace Days_Gone;

public class DaysGone : Mod
{
    private int totalDays;
    private DaysGoneConfig config;
    private readonly DaysCalculationService _calculationService = new();

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

    private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
    {
        // Calculate current day count
        totalDays = _calculationService.CalculateTotalDays();
    }

    private void OnDayStarted(object sender, DayStartedEventArgs e)
    {
        totalDays = _calculationService.CalculateTotalDays();
    }

    private void OnRenderingHud(object sender, RenderingHudEventArgs e)
    {
        if (!Context.IsWorldReady) return;

        string text = $"{this.Helper.Translation.Get("dayCounter.text")} {totalDays}";

        if(config.ShowSeasonYear) text += $"\n{Game1.currentSeason} Y{Game1.year}";

        Vector2 position = new(config.PositionX, config.PositionY);
        Color color = GetColorFromString(config.TextColor);

        Game1.spriteBatch.DrawString(Game1.smallFont, text, position, color);
    }

    private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
    {
        TryRegisterConfigMenu();
    }

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

    private void TryRegisterConfigMenu()
    {
        // Check if GMCM is installed and available
        if (this.Helper.ModRegistry.IsLoaded("spacechase0.GenericModConfigMenu")) return;

        try
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu != null)
            {
                configMenu.Register(
                    mod: this.ModManifest,
                    reset: () => this.config = new DaysGoneConfig(),
                    save: () => this.Helper.WriteConfig(this.config)
                );

                configMenu.AddNumberOption(
                    mod: this.ModManifest,
                    name: () => this.Helper.Translation.Get("config.xPosition.name"),
                    getValue: () => this.config.PositionX,
                    setValue: value => this.config.PositionX = value,
                    min: 0,
                    max: Game1.viewport.Width
                );

                configMenu.AddNumberOption(
                    mod: this.ModManifest,
                    name: () => this.Helper.Translation.Get("config.yPosition.name"),
                    getValue: () => this.config.PositionY,
                    setValue: value => this.config.PositionY = value,
                    min: 0,
                    max: Game1.viewport.Height
                );

                configMenu.AddTextOption(
                    mod: this.ModManifest,
                    name: () => this.Helper.Translation.Get("config.textColor.name"),
                    getValue: () => this.config.TextColor,
                    setValue: value => this.config.TextColor = value,
                    allowedValues: new[] { "White", "Red", "Blue", "Green", "Black", "Yellow" }
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => this.Helper.Translation.Get("config.showSeasonYear.name"),
                    getValue: () => this.config.ShowSeasonYear,
                    setValue: value => this.config.ShowSeasonYear = value
                );
            }
        }
        catch(Exception ex)
        {
            this.Monitor.Log($"Failed loading GMCM: {ex.Message}", LogLevel.Warn);
        }
    }
}
