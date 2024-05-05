using tobeh.Valmar.Server.Util.NChunkTree.Drops;

namespace tobeh.Valmar.Server.Util;

public static class DropHelper
{
    public const string DropTimestampFormat = "yyyy-MM-dd HH:mm:ss";

    public static DateTimeOffset ParseDropTimestamp(string timestamp)
    {
        return DateTimeOffset.ParseExact(timestamp, DropTimestampFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
    }
    public static string FormatDropTimestamp(DateTimeOffset timestamp)
    {
        return timestamp.ToString(DropTimestampFormat);
    }

    public static double Weight(double catchMs)
    {
        if (catchMs < 0) return 0;
        if (catchMs > 1000) return 0.3;
        var weight =  -1.78641975945623 * Math.Pow(10, -9) * Math.Pow(catchMs, 4) + 0.00000457264006980028 * Math.Pow(catchMs, 3) - 0.00397188791256729 * Math.Pow(catchMs, 2) + 1.21566760222325 * catchMs;
        return weight / 100;
    }
}