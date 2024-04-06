namespace Valmar.Domain.Classes;

public record DropboostDdo(int Login, int Value, DateTimeOffset StartDate, DateTimeOffset EndDate, DateTimeOffset CooldownEndDate, int DurationSeconds, double Factor, int CooldownSeconds);