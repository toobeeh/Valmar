namespace tobeh.Valmar.Server.Domain;

public interface IDropsDomainService
{
    Task ScheduleDrop(int delaySeconds, int? eventDropId);
    Task<Tuple<int, int>> CalculateDropDelayBounds(int playerCount, double boostFactor);
    Task<double> GetCurrentDropBoost();
}