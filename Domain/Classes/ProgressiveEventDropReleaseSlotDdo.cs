namespace Valmar.Domain.Classes;

public record ProgressiveEventDropReleaseSlotDdo(DateTimeOffset Start, DateTimeOffset End, int EventDropId, bool IsReleased);