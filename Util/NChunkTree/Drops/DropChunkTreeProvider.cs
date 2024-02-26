using System.Collections.Concurrent;
using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

public record PersistentDropChunkRange(long? Start, long? End, DateTimeOffset? StartDate, DateTimeOffset? EndDate);

public class CachedDropChunkContext
{
    public readonly ConcurrentDictionary<string, UserStore<string, double>> LeagueDropValue = new ();
    public readonly ConcurrentDictionary<string, UserStore<string, int>> LeagueDropCount = new ();
    public readonly ConcurrentDictionary<string, UserStore<string, StreakResult>> LeagueStreak = new ();
    public readonly ConcurrentDictionary<string, UserStore<string, double>> LeagueAverageTime = new ();
    public readonly ConcurrentDictionary<string, UserStore<string, EventResult>> EventDetails = new ();
    public readonly ConcurrentDictionary<string, UserStore<string, IList<string>>> LeagueParticipants = new ();
}

public class DropChunkTreeProvider : NChunkTreeProvider
{
    private int? _rootNode;
    
    public readonly ConcurrentDictionary<int, PersistentDropChunkRange> PersistentChunkContext = new();
    public readonly ConcurrentDictionary<int, CachedDropChunkContext> CachedChunkContext = new();
    
    /// <summary>
    /// Creates a new leaf which can be added to the drop tree
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="dropStart"></param>
    /// <param name="dropEnd"></param>
    /// <returns></returns>
    public PersistentDropChunk CreateLeaf(IServiceProvider provider, long? dropStart, long? dropEnd)
    {
        var leaf = CreateNode<IDropChunk, DropChunkTreeProvider, PersistentDropChunk>(provider, 0, 1);
        leaf.SetChunkSize(dropStart, dropEnd);
        return leaf;
    }

    /// <summary>
    /// Gets an instance of the drop chunk tree
    /// All instances share the same context (structure and cached data), but are independently constructed by DI
    /// 
    /// The instance is completely thread-safe:
    /// Structures in cacheddropcontext are thread-safe, and every access to the sub-nodes in a node
    /// will result in new instances of the nodes with new injected DI services
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
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