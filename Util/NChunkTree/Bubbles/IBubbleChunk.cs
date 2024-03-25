namespace Valmar.Util.NChunkTree.Bubbles;

public interface IBubbleChunk
{
    int? TraceIdStart { get; }
    int? TraceIdEnd { get; }
    DateTimeOffset? TraceTimestampStart { get; }
    DateTimeOffset? TraceTimestampEnd { get; }
    Task<List<int>> EvaluateSubChunks(int chunkSize);
    
    DateTimeOffset GetFirstSeenDate(int login);
}