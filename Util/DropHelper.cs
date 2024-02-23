namespace Valmar.Util;

public static class DropHelper
{
    public static readonly string DropTimestampFormat = "yyyy-MM-dd HH:mm:ss";
    public static DateTimeOffset ParseDropTimestamp(string timestamp)
    {
        return DateTimeOffset.ParseExact(timestamp, DropTimestampFormat, System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string FormatDropTimestamp(DateTimeOffset timestamp)
    {
        return timestamp.ToString(DropTimestampFormat);
    }
}