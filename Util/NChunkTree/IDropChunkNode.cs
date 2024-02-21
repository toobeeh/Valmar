namespace Valmar.Util.NChunkTree;

public interface IDropChunkNode
{
    public long DropIndexStart { get; }
    public long DropIndexEnd { get; }
    public double GetTotalDropValueForUser(string id);
}