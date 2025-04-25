using StardewModdingAPI;

namespace Days_Gone.Helper;

public static class ColorHelper
{
    /// <summary>Returns all allowed colors</summary>
    public static string[] GetAllowedColors()
    {
        return new[] { "White", "Red", "Blue", "Green", "Black", "Yellow" };
    }

    /// <summary>Get the Translation for the chosen Color</summary>
    /// <param name="colorName">The Name of the color</param>
    /// <param name="helper">The Mod Helper to work with</param>
    public static string GetTranslatedColor(string colorName, IModHelper helper)
    {
        return helper.Translation.Get(colorName.ToLower());
    }
}
