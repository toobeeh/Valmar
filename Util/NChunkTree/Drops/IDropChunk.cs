using System.Collections.Concurrent;

namespace Valmar.Util.NChunkTree.Drops;

public interface IDropChunk
{
    long? DropIndexStart { get; }
    long? DropIndexEnd { get; }
    DateTimeOffset? DropTimestampStart { get; }
    DateTimeOffset? DropTimestampEnd { get; }
    Task<double> GetLeagueWeight(string id);
    Task<double> GetLeagueWeight(string id, DateTimeOffset? start, DateTimeOffset? end);
    Task<int> GetLeagueCount(string id);
    Task<int> GetLeagueCount(string id, DateTimeOffset? start, DateTimeOffset? end);
    Task<IList<string>> GetLeagueParticipants();
    Task<IList<string>> GetLeagueParticipants(DateTimeOffset? start, DateTimeOffset? end);
    Task<EventResult> GetEventLeagueDetails(int[] eventDropIds, string userId, int userLogin);
    Task<List<long>> EvaluateSubChunks(int chunkSize);
    Task<Dictionary<string, LeagueResult>> GetLeagueResults(DateTimeOffset? start, DateTimeOffset? end);
    Task MarkEventDropsAsRedeemed(string userId, List<long> dropIds);
    
}

public record StreakResult(int Tail, int Head, int Streak);
public record EventResult(ConcurrentDictionary<int, ConcurrentDictionary<long, double>> RedeemableCredit, double Progress);
public record LeagueResult(string Id, double Score, int Count, double AverageTime, double AverageWeight, StreakResult Streak);