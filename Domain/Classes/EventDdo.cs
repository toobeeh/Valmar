namespace Valmar.Domain.Classes;

public record EventDdo(string Name, int Id, string Description, int Length, bool Progressive, DateTimeOffset StartDate, DateTimeOffset EndDate);