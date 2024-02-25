using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

public class DropChunkLeaf : NChunkTree<IDropChunk>
{
    private static int ct = 0;
    private long? _dropIndexStart, _dropIndexEnd;
    
    private readonly IServiceProvider _services;
    private readonly PersistentDropChunk _chunk;
    protected override long? ChunkStartIndex => Chunk.DropIndexStart;
    protected override long? ChunkEndIndex => Chunk.DropIndexEnd;

    public sealed override IDropChunk Chunk
    {
        get
        {
            Console.WriteLine(ct++); 
            var chunk = ActivatorUtilities.CreateInstance<PersistentDropChunk>(_services);
            chunk.SetChunkSize(_dropIndexStart, _dropIndexEnd);
            return chunk;
        }
    }

    public DropChunkLeaf(IServiceProvider services) : base(0)
    {
        /*_chunk = ActivatorUtilities.CreateInstance<PersistentDropChunk>(services);*/
        _services = services;
    }
    public DropChunkLeaf WithChunkSize(long? dropIndexStart, long? dropIndexEnd) //  careful - might destroy integrity of index range if used in a tree!
    {
        _dropIndexStart = dropIndexStart;
        _dropIndexEnd = dropIndexEnd;
        /*_chunk.SetChunkSize(dropIndexStart, dropIndexEnd);*/
        return this;
    } 
    protected override NChunkTree<IDropChunk> CreateExpansionNode()
    {
        throw new InvalidOperationException("Leaves cannot expand");
    }
}