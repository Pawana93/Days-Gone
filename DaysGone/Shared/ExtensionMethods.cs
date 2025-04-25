namespace Days_Gone.Shared;

/**
 * <summary>
 * A collection of Extension methods, to easier work with functions
 * </summary>
 **/
public static class ExtensionMethods
{
    public static string FirstToUpper(this string str)
    {
        if (str == null) return null;

        if (str.Length > 1) return char.ToUpper(str[0]) + str.Substring(1);

        return str.ToUpper();
    }
}
