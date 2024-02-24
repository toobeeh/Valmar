using Valmar.Util.NChunkTree.Drops;

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

    public static List<long> FindDropToRedeem(EventResult eventResult, int amount, int? eventDropid = null)
    {
        List<Tuple<long, double>> candidates;
        if (eventDropid is int id)
        {
            candidates = eventResult.RedeemableCredit[id]
                .Keys
                .Select(key => new Tuple<long, double>(key, eventResult.RedeemableCredit[id][key]))
                .ToList();
        }
        else
        {
            candidates = eventResult.RedeemableCredit
                .Keys
                .SelectMany(key => eventResult.RedeemableCredit[key]
                    .Keys
                    .Select(subkey => new Tuple<long, double>(subkey, eventResult.RedeemableCredit[key][subkey])))
                .ToList();
        }
        
        candidates.Sort((a, b) => a.Item2.CompareTo(b.Item2));

        List<Tuple<long, double>> currentSet = new List<Tuple<long, double>>();
        List<Tuple<long, double>> optimalSet = null;

        double currentSum = 0;
        double optimalSum = double.MaxValue;

        int startIndex = 0;
        int endIndex = 0;

        while (startIndex < candidates.Count)
        {
            // Try to add candidates to the current set until the sum is greater than or equal to the amount
            while (endIndex < candidates.Count && currentSum < amount)
            {
                currentSum += candidates[endIndex].Item2;
                currentSet.Add(candidates[endIndex]);
                endIndex++;
            }

            // Check if the current set is a valid solution and has a lower sum
            if (currentSum >= amount && currentSum < optimalSum)
            {
                optimalSet = new List<Tuple<long, double>>(currentSet);
                optimalSum = currentSum;
            }

            // Remove the first element from the current set to try the next combination
            currentSum -= currentSet[0].Item2;
            currentSet.RemoveAt(0);
            startIndex++;
        }

        return optimalSet.Select(item => item.Item1).ToList();
    }
}