namespace Valmar.Util.NChunkTree;

public interface IDropChunk
{
    long? DropIndexStart { get; }
    long? DropIndexEnd { get; }
    Task<double> GetTotalDropValueForUser(string id);
}