namespace Valmar.Util;

public static class BubbleHelper
{
    public static readonly string[] TraceTimestampFormat = ["dd/MM/yyyy", "dd.MM.yyyy"];
    public static DateTimeOffset ParseTraceTimestamp(string timestamp)
    {
        return DateTimeOffset.ParseExact(timestamp, TraceTimestampFormat, System.Globalization.CultureInfo.InvariantCulture);
    }
    public static string FormatTraceTimestamp(DateTimeOffset timestamp, bool oldFormat = false)
    {
        return timestamp.ToString(TraceTimestampFormat[oldFormat ? 1 : 0]);
    }
}