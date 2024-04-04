namespace Valmar.Domain.Classes;

public record DropboostDdo(int Login, int Value, DateTimeOffset StartDate, int DurationSeconds, double Factor, int CooldownSeconds);