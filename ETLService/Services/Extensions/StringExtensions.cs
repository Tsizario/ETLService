namespace ETLService.Services.Extensions;

public static class StringExtentions
{
    public static string JoinString<T>(this IEnumerable<T> values, string separator = " ")
        => string.Join(separator, values);
}