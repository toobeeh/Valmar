using System.Collections;
using System.Collections.Concurrent;
using Valmar.Database;

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
    Task<StreakResult> GetLeagueStreak(string id);
    Task<StreakResult> GetLeagueStreak(string id, DateTimeOffset? start, DateTimeOffset? end);
    Task<double> GetLeagueAverageTime(string id);
    Task<double> GetLeagueAverageTime(string id, DateTimeOffset? start, DateTimeOffset? end);
    Task<EventResult> GetEventLeagueDetails(int eventId, string userId, int userLogin);
    Task<List<long>> EvaluateSubChunks(int chunkSize);
}

public record StreakResult(int Tail, int Head, int Streak);
public record EventResult(ConcurrentDictionary<int, ConcurrentDictionary<long, double>> RedeemableCredit, double Progress);