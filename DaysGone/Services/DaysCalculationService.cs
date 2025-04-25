using StardewValley;

namespace Days_Gone.Services;

public class DaysCalculationService
{
    /// <summary>
    /// Calculates the total number of days in the game.
    /// </summary>
    /// <returns>The amount of days passed until the current day</returns>
    public int CalculateTotalDays()
    {
        return (Game1.year - 1) * 112 +
            Utility.getSeasonNumber(Game1.currentSeason) * 28 +
            Game1.dayOfMonth;
    }
}
