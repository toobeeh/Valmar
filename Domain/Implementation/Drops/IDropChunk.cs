namespace Valmar.Domain.Implementation.Drops;

public interface IDropChunk
{
    long? DropIndexStart { get; }
    long? DropIndexEnd { get; }
    Task<double> GetTotalLeagueWeight(string id);
    Task<double> GetLeagueWeightInTimespan(string id, DateOnly start, DateOnly end);
}