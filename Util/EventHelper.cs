using Valmar.Domain.Classes;

namespace Valmar.Util;

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
}