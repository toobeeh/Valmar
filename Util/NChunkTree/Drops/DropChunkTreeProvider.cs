using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

public record PersistentDropChunkRange(long? Start, long? End, DateTimeOffset? StartDate, DateTimeOffset? EndDate);

public class CachedDropChunkContext
{
    public readonly ConcurrentDictionary<string, KVStore<string, double>> LeagueDropValue = new ();
    public readonly ConcurrentDictionary<string, KVStore<string, int>> LeagueDropCount = new ();
    public readonly ConcurrentDictionary<string, KVStore<string, StreakResult>> LeagueStreak = new ();
    public readonly ConcurrentDictionary<string, KVStore<string, double>> LeagueAverageTime = new ();
    public readonly ConcurrentDictionary<string, KVStore<string, EventResult>> EventDetails = new ();
    public readonly ConcurrentDictionary<string, KVStore<string, IList<string>>> LeagueParticipants = new ();
    public readonly ConcurrentDictionary<string, KVStore<string, Dictionary<string, LeagueResult>>> LeagueResults = new ();
}

public class DropChunkTreeProvider(ILogger<DropChunkTreeProvider> logger) : NChunkTreeProvider
{
    private int? _rootNode;
    private readonly object _treeStructureLock = new();
    
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
    
    public void AddLeaf(IServiceProvider provider, PersistentDropChunk leaf)
    {
        if (_rootNode is { } rootNodeValue)
        {
            lock (_treeStructureLock)
            {
                var tree = (CachedDropChunk)GetNode<IDropChunk, DropChunkTreeProvider>(provider, rootNodeValue);
                tree.AddChunk(leaf);
            }
        }
        else throw new InvalidOperationException("Tree has not been initialized");
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
        CachedDropChunk tree;
        lock (_treeStructureLock)
        {
            if(_rootNode is {} rootNodeValue)
            {
                tree = (CachedDropChunk) GetNode<IDropChunk, DropChunkTreeProvider>(provider, rootNodeValue);
                return tree;
            }
            
        
            // tree has not been created/inited yet - do now
            var config = provider.GetRequiredService<IOptions<DropChunkConfiguration>>().Value;
            tree = CreateNode<IDropChunk, DropChunkTreeProvider, CachedDropChunk>(provider, config.BranchingCoefficient, 1);
            _rootNode = tree.NodeId;
        }
    
        // add a single leaf which covers the whole range, and repartition tree to build proper chunksizes
        var leaf = CreateLeaf(provider, null, null);
        AddLeaf(provider, leaf);
        RepartitionTree(tree);
        
        return tree;
    }

    public void RepartitionTree(CachedDropChunk tree)
    {
        var sw = new Stopwatch();
        sw.Start();
        tree.RepartitionChunks();
        sw.Stop();
        logger.LogInformation("Repartitioned tree in {time}ms", sw.ElapsedMilliseconds);
    }
    
    public override void RemoveNode(int nodeId, int? parentNode)
    {
        base.RemoveNode(nodeId, parentNode);
        PersistentChunkContext.TryRemove(nodeId, out _);
        CachedChunkContext.TryRemove(nodeId, out _);
        
        // dirty parent node
        if(parentNode is {} parentNodeValue)
        {
            PersistentChunkContext.TryRemove(parentNodeValue, out _);
            CachedChunkContext.TryRemove(parentNodeValue, out _);
        }
    }
}