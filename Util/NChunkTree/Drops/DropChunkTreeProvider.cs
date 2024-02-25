using System.Collections.Concurrent;
using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

public record PersistentDropChunkRange(long? Start, long? End, DateTimeOffset? StartDate, DateTimeOffset? EndDate);

public class DropChunkTreeProvider : NChunkTreeProvider
{
    private int? _rootNode;
    public readonly ConcurrentDictionary<int, PersistentDropChunkRange> ChunkRanges = new();
    public PersistentDropChunk CreateLeaf(IServiceProvider provider, long? dropStart, long? dropEnd)
    {
        var leaf = CreateNode<IDropChunk, DropChunkTreeProvider, PersistentDropChunk>(provider, 0, 1);
        leaf.SetChunkSize(dropStart, dropEnd);
        return leaf;
    }

    public CachedDropChunk GetTree(IServiceProvider provider)
    {
        if(_rootNode is {} rootNodeValue)
        {
            return (CachedDropChunk) GetNode<IDropChunk, DropChunkTreeProvider>(provider, rootNodeValue);
        }
        
        var tree = CreateNode<IDropChunk, DropChunkTreeProvider, CachedDropChunk>(provider, 2, 1);
        _rootNode = tree.NodeId;
        return tree;
    }
}