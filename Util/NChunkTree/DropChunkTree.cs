namespace Valmar.Util.NChunkTree;

public class DropChunkTree(int chunkCount) : NChunkTree<IDropChunkNode>(chunkCount), IDropChunkNode
{
    protected override long ChunkStartIndex => DropIndexStart;
    protected override long ChunkEndIndex => DropIndexEnd;
    protected override NChunkTree<IDropChunkNode> CreateExpansionNode()
    {
        return new DropChunkTree(ChunkNumber);
    }

    protected override IDropChunkNode Chunk => this;

    public long DropIndexStart => Chunks.First().DropIndexStart;
    public long DropIndexEnd => Chunks.Last().DropIndexEnd;
    public double GetTotalDropValueForUser(string id)
    {
        return Chunks.Sum(c => c.GetTotalDropValueForUser(id));
    }
}