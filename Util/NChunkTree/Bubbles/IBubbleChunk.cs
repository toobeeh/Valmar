namespace Valmar.Util.NChunkTree.Bubbles;

public interface IBubbleChunk
{
    int? TraceIdStart { get; }
    int? TraceIdEnd { get; }
    DateTimeOffset? TraceTimestampStart { get; }
    DateTimeOffset? TraceTimestampEnd { get; }
    Task<List<int>> EvaluateSubChunks(int chunkSize);
    
    Task<DateTimeOffset?> GetFirstSeenDate(int login);
    Task<BubbleTimespanRangeDdo> GetAmountCollectedInTimespan(int login, DateTimeOffset? start, DateTimeOffset? end);
}

public record BubbleTimespanRangeDdo(int? StartAmount, int? EndAmount);