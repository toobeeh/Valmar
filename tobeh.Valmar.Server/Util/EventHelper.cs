using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Util;

public static class EventHelper
{
    public static readonly string[] EventTimestampFormats = ["dd/MM/yyyy", "dd.MM.yyyy"];
    public static DateTimeOffset ParseEventTimestamp(string timestamp)
    {
        return DateTimeOffset.ParseExact(timestamp, EventTimestampFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);
    }

    public static int GetEventScenePrice(int eventDayLength)
    {
        return 500 * eventDayLength;
    }
    
    public static List<ProgressiveEventDropReleaseSlotDdo> GetProgressiveEventDropReleaseSlots(DateTimeOffset eventStart, int eventDuration, List<int> eventDropIds)
    {
        var daysPerDrop = eventDuration / eventDropIds.Count;
        var lastDropRemainder = eventDuration % eventDropIds.Count;

        var daySplits = Enumerable.Repeat(daysPerDrop, eventDropIds.Count).ToList();
        for(var i = 0; i<daySplits.Count && lastDropRemainder > 0; i++)
        {
            daySplits[i]++;
            lastDropRemainder--;
        }
        daySplits.Reverse(); // let last sprites have more time

        var slots = eventDropIds.Select((id, idx) =>
        {
            var slotStart = eventStart.AddDays(daySplits.Take(idx).Sum());
            var slotEnd = slotStart.AddDays(daySplits[idx]);
            var released = DateTimeOffset.UtcNow > slotStart;
            return new ProgressiveEventDropReleaseSlotDdo(
                slotStart,
                slotEnd,
                id,
                released
            );
        }).ToList();

        return slots;
    }
    
    public static double CalculateCurrentGiftLossRate(double required, double collected)
    {
        var ratio = collected / required;

        var loss = ratio / 5 + 0.1;
        if (loss < 0.2) loss = 0.2;
        if (loss > 0.8) loss = 0.8;

        return loss;
    }
    
    public static double CalculateRandomGiftLoss(double lossBase, int amount)
    {
        var lossMin = Convert.ToInt32(Math.Round(lossBase * amount * 0.7));
        var lossMax = Convert.ToInt32(Math.Round(lossBase * amount * 1.1));
        
        var loss = 0;
        if (lossMax <= 1)
        {
            var loseProb = new Random().NextDouble();
            if (loseProb <= lossBase) loss = 1;
        }
        else
        {
            loss = new Random().Next(lossMin, lossMax + 1);
        }

        return loss;
    }
}