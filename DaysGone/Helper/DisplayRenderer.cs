using Days_Gone.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System.Runtime.CompilerServices;

namespace Days_Gone.Helper;

public static class DisplayRenderer
{
    private static Texture2D _scrollTexture;
    private static Texture2D _textBoxTexture;
    private static Rectangle _textBoxSourceRect;

    public static void LoadTextures(IModHelper helper)
    {
        //_scrollTexture = helper.GameContent.Load<Texture2D>(@"LooseSprites\\scroll");
        _textBoxTexture = helper.GameContent.Load<Texture2D>(@"LooseSprites\\textBox");
        _textBoxSourceRect = new Rectangle(0, 0, _textBoxTexture.Width, _textBoxTexture.Height);
    }

    public static void DrawDisplay(DaysGoneConfig config, string text, SpriteBatch spriteBatch)
    {
        Vector2 textSize = Game1.smallFont.MeasureString(text);
        Vector2 position = new Vector2(config.PositionX, config.PositionY);
        Color textColor = ColorHelper.GetColorFromString(config.TextColor);

        switch(config.DisplayStyle)
        {
            case DisplayStyle.TextOnly:
                spriteBatch.DrawString(Game1.smallFont, text, position, textColor);
                break;
            case DisplayStyle.TextBox:
                DrawTextBox(spriteBatch, position, textSize, text, textColor, config.BackgroundOpacity);
                break;
            case DisplayStyle.Scroll:
                DrawScroll(spriteBatch, position, textSize, text, textColor, config.BackgroundOpacity);
                break;
        }
    }

    private static void DrawTextBox(SpriteBatch spriteBatch, Vector2 position, Vector2 textSize,
                                      string text, Color textColor, float opacity)
    {
        /*// Create the Background
        Rectangle background = new(
            (int)position.X - 10,
            (int)position.Y - 5,
            (int)textSize.X + 20,
            (int)textSize.Y + 10
        );

        // Draw Texture or colored background
        if(_textBoxTexture != null)
        {
            spriteBatch.Draw(_textBoxTexture, background, Color.White * opacity);
        }
        else
        {
            spriteBatch.Draw(Game1.staminaRect, background, Color.Black * opacity);
            spriteBatch.Draw(Game1.staminaRect,
                new Rectangle(background.X + 2, background.Y + 2, background.Width - 4, background.Height - 4),
                Color.White * opacity);
        }

        // Draw the Text
        spriteBatch.DrawString(Game1.smallFont, text, position, textColor);*/

        int width = 300;
        int height = (int)textSize.Y + 30;

        IClickableMenu.drawTextureBox(
            spriteBatch,
            texture: _textBoxTexture,
            sourceRect: _textBoxSourceRect,
            x: (int)position.X - 15,
            y: (int)position.Y - 10,
            width: width,
            height: height,
            color: Color.White * opacity,
            draw_layer: 1f,
            drawShadow: false
        );

        Vector2 textPos = new Vector2(
            position.X + (width - textSize.X) / 2 - 15,
            position.Y
        );

        spriteBatch.DrawString(Game1.smallFont, text, textPos, textColor);
    }

    private static void DrawScroll(SpriteBatch spriteBatch, Vector2 position, Vector2 textSize,
                                     string text, Color textColor, float opacity)
    {
        /*if (_scrollTexture == null) return;

        // Adjust Scroll Size
        Rectangle scrollRect = new Rectangle(
            (int)position.X - 20,
            (int)position.Y - 15,
            _scrollTexture.Width,
            _scrollTexture.Height);

        spriteBatch.Draw(_scrollTexture, scrollRect, Color.White * opacity);

        // Center Text on Scroll
        Vector2 textPosition = new Vector2(
            scrollRect.X + (scrollRect.Width - textSize.X) / 2,
            scrollRect.Y + (scrollRect.Height - textSize.Y) / 2);

        spriteBatch.DrawString(Game1.smallFont, text, textPosition, textColor);*/

        if (_scrollTexture == null) return;

        float scale = 0.8f;
        Rectangle scrollRect = new Rectangle(
            (int)position.X - (int)(_scrollTexture.Width * scale) / 2,
            (int)position.Y - 10,
            (int)(_scrollTexture.Width * scale),
            (int)(_scrollTexture.Height * scale)
        );

        spriteBatch.Draw(
            texture: _scrollTexture,
            destinationRectangle: scrollRect,
            sourceRectangle: null,
            color: Color.White * opacity,
            rotation: 0f,
            origin: Vector2.Zero,
            effects: SpriteEffects.None,
            layerDepth: 1f
        );

        Vector2 textPos = new Vector2(
            scrollRect.X + (scrollRect.Width - textSize.X) / 2,
            scrollRect.Y + (scrollRect.Height - textSize.Y) / 2
        );

        spriteBatch.DrawString(Game1.smallFont, text, textPos, textColor);
    }
}
