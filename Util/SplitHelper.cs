using Valmar.Domain.Exceptions;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Util;

public static class SplitHelper
{
    public static readonly int FactorSplitCost = 2;
    public static readonly double FactorIncrease = 0.1;
    public static readonly int DurationSplitCost = 1;
    public static readonly int DurationIncreaseMinutes = 20;
    public static readonly int CooldownSplitCost = 1;
    public static readonly int CooldownIncreaseHours = 12;
    public static readonly double DefaultFactor = 1;
    public static readonly int DefaultDurationMinutes = 60;
    public static readonly int DefaultCooldownHours = 0;
    
    public static readonly string[] SplitTimestampFormats = ["dd/MM/yyyy", "dd.MM.yyyy"];
    public static DateTimeOffset ParseSplitTimestamp(string timestamp)
    {
        return DateTimeOffset.ParseExact(timestamp, SplitTimestampFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
    }
    public static string FormatSplitTimestamp(DateTimeOffset timestamp)
    {
        return timestamp.ToString(SplitTimestampFormats[0]);
    }
    
    public static int CalculateSplitCost(double factor, int durationS, int cooldownS)
    {
        var exact = (factor - DefaultFactor) / FactorIncrease * FactorSplitCost
                    + (durationS - DefaultDurationMinutes * 60) / (DurationIncreaseMinutes * 60) * DurationSplitCost
                    + (cooldownS - DefaultCooldownHours * 3600) / 3600 / CooldownIncreaseHours * CooldownSplitCost;

        return Convert.ToInt32(exact);
    }

    public static double CalculateFactorBoost(int splitsCount)
    {
        if(splitsCount % FactorSplitCost != 0)
        {
            throw new UserOperationException($"Invalid split count ({splitsCount}) provided for factor boost");
        }
        
        return DefaultFactor + splitsCount / FactorSplitCost * FactorIncrease;
    }
    
    public static int CalculateDurationSecondsBoost(int splitsCount)
    {
        if(splitsCount % DurationSplitCost != 0)
        {
            throw new UserOperationException($"Invalid split count ({splitsCount}) provided for duration boost");
        }
        
        return (DefaultDurationMinutes + splitsCount / DurationSplitCost * DurationIncreaseMinutes) * 60;
    }
    
    public static int CalculateCooldownSecondsBoost(int splitsCount)
    {
        if(splitsCount % CooldownSplitCost != 0)
        {
            throw new UserOperationException($"Invalid split count ({splitsCount}) provided for cooldown boost");
        }
        
        return (DefaultCooldownHours + splitsCount / CooldownSplitCost * CooldownIncreaseHours) * 3600;
    }
}