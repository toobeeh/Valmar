using System.Collections.Concurrent;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Util.NChunkTree.Drops;

public interface IDropChunk
{
    long? DropIndexStart { get; }
    long? DropIndexEnd { get; }
    DateTimeOffset? DropTimestampStart { get; }
    DateTimeOffset? DropTimestampEnd { get; }
    Task<int> GetLeagueCount(string id);
    Task<int> GetLeagueCount(string id, DateTimeOffset? start, DateTimeOffset? end);
    Task<IList<string>> GetLeagueParticipants();
    Task<IList<string>> GetLeagueParticipants(DateTimeOffset? start, DateTimeOffset? end);
    Task<Dictionary<string, LeagueResult>> GetLeagueResults(DateTimeOffset? start, DateTimeOffset? end);
    Task<EventResultDdo> GetEventLeagueDetails(int[]? eventDropIds, string userId);
    Task<List<long>> EvaluateSubChunks(int chunkSize);
}
public record StreakResult(int Tail, int Head, int Streak);

public record LeagueResult(string Id, double Score, int Count, double AverageTime, double AverageWeight, StreakResult Streak);