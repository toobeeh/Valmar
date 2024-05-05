namespace tobeh.Valmar.Server.Domain.Classes;

public record EventResultDdo(double TotalCollected, Dictionary<int, double> EventDropProgress);