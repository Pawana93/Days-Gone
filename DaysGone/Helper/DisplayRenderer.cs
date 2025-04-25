using Days_Gone.Config;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;

namespace Days_Gone.Helper;

public static class DisplayRenderer
{
    private static Texture2D _scrollTexture;
    private static Texture2D _textBoxTexture;

    public static void LoadTextures(IModHelper helper)
    {
        _scrollTexture = helper.GameContent.Load<Texture2D>("assets/scroll.png");
        _textBoxTexture = helper.GameContent.Load<Texture2D>("assets/textbox.png");
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
        // Create the Background
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
        spriteBatch.DrawString(Game1.smallFont, text, position, textColor);
    }

    private static void DrawScroll(SpriteBatch spriteBatch, Vector2 position, Vector2 textSize,
                                     string text, Color textColor, float opacity)
    {
        if (_scrollTexture == null) return;

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

        spriteBatch.DrawString(Game1.smallFont, text, textPosition, textColor);
    }
}
