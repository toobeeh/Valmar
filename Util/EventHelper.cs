namespace Valmar.Util;

public static class EventHelper
{
    public static readonly string[] EventTimestampFormats = ["dd/MM/yyyy", "dd.MM.yyyy"];
    public static DateTimeOffset ParseEventTimestamp(string timestamp)
    {
        return DateTimeOffset.ParseExact(timestamp, EventTimestampFormats, System.Globalization.CultureInfo.InvariantCulture);
    }
}