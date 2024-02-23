using Valmar.Database;

namespace Valmar.Util.NChunkTree.Drops;

public interface IDropChunk
{
    long? DropIndexStart { get; }
    long? DropIndexEnd { get; }
    DateTimeOffset? DropTimestampStart { get; }
    DateTimeOffset? DropTimestampEnd { get; }
    Task<double> GetTotalLeagueWeight(string id);
    Task<double> GetLeagueWeightInTimespan(string id, DateTimeOffset start, DateTimeOffset end);
    Task<int> GetTotalLeagueCount(string id);
    Task<int> GetLeagueCountInTimespan(string id, DateTimeOffset start, DateTimeOffset end);
}