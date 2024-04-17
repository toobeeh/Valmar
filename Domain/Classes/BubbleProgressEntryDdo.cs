namespace Valmar.Domain.Classes;

public enum BubbleProgressIntervalModeDdo
{
    Day,
    Week,
    Month
}
public record BubbleProgressEntryDdo(DateTimeOffset Date, int Bubbles);