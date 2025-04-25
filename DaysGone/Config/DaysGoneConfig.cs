namespace Days_Gone.Config;

/**
 * <summary>The Config File for use with the GenericModConfigMenu</summary>
 **/
public class DaysGoneConfig
{
    public int PositionX { get; set; } = 20;
    public int PositionY { get; set; } = 20;
    public string TextColor { get; set; } = "White";
    public bool ShowSeasonYear { get; set; } = false;

    public DisplayStyle DisplayStyle { get; set; } = DisplayStyle.TextOnly;
    public float BackgroundOpacity { get; set; } = 0.8f;
}

public enum DisplayStyle
{
    TextOnly,
    TextBox,
    Scroll
}
