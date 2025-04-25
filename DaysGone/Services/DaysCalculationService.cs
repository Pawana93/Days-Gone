using StardewValley;

namespace Days_Gone.Services;

public class DaysCalculationService
{
    public int CalculateTotalDays()
    {
        return (Game1.year - 1) * 112 +
            Utility.getSeasonNumber(Game1.currentSeason) * 28 +
            Game1.dayOfMonth;
    }
}
