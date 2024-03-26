using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using Valmar.Domain.Implementation.Bubbles;
using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Bubbles;

public record PersistentBubbleChunkRange(int? TraceIdStart, int? TraceIdEnd, DateTimeOffset? StartDate, DateTimeOffset? EndDate);

public class CachedBubbleChunkContext
{
    public readonly ConcurrentDictionary<string, KVStore<string, DateTimeOffset?>> FirstSeenDates = new ();
    public readonly ConcurrentDictionary<string, KVStore<string, BubbleTimespanRangeDdo>> CollectedBubbles = new ();
}

public class BubbleChunkTreeProvider(ILogger<BubbleChunkTreeProvider> logger, IServiceProvider provider) : NChunkTreeProvider(provider)
{
    private int? _rootNode;
    private readonly object _treeStructureLock = new();
    
    public readonly ConcurrentDictionary<int, PersistentBubbleChunkRange> PersistentChunkContext = new();
    public readonly ConcurrentDictionary<int, CachedBubbleChunkContext> CachedChunkContext = new();
    
    /// <summary>
    /// Creates a new leaf which can be added to the bubble tree
    /// </summary>
    /// <param name="traceIdStart"></param>
    /// <param name="traceIdEnd"></param>
    /// <returns></returns>
    public PersistentBubbleChunk CreateLeaf(int? traceIdStart, int? traceIdEnd)
    {
        var leaf = CreateNode<IBubbleChunk, BubbleChunkTreeProvider, PersistentBubbleChunk>(0, 1);
        leaf.SetChunkSize(traceIdStart, traceIdEnd);
        return leaf;
    }
    
    public void AddLeaf(PersistentBubbleChunk leaf)
    {
        if (_rootNode is { } rootNodeValue)
        {
            lock (_treeStructureLock)
            {
                var tree = (CachedBubbleChunk)GetNode<IBubbleChunk, BubbleChunkTreeProvider>(rootNodeValue);
                tree.AddChunk(leaf);
            }
        }
        else throw new InvalidOperationException("Tree has not been initialized");
    }

    /// <summary>
    /// Gets an instance of the bubble chunk tree
    /// All instances share the same context (structure and cached data), but are independently constructed by DI
    /// 
    /// The instance is completely thread-safe:
    /// Structures in cachedbubblecontext are thread-safe, and every access to the sub-nodes in a node
    /// will result in new instances of the nodes with new injected DI services
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public CachedBubbleChunk GetTree()
    {
        
        CachedBubbleChunk tree;
        lock (_treeStructureLock)
        {
            if(_rootNode is {} rootNodeValue)
            {
                tree = (CachedBubbleChunk) GetNode<IBubbleChunk, BubbleChunkTreeProvider>(rootNodeValue);
                return tree;
            }
            
            // tree has not been created/inited yet - do now
            var prov = CreateScopedServices();
            var config = prov.GetRequiredService<IOptions<BubbleChunkConfiguration>>().Value;
            tree = CreateNode<IBubbleChunk, BubbleChunkTreeProvider, CachedBubbleChunk>(config.BranchingCoefficient, 1);
            _rootNode = tree.NodeId;
        }
    
        // add a single leaf which covers the whole range, and repartition tree to build proper chunksizes
        var leaf = CreateLeaf(null, null);
        AddLeaf(leaf);
        RepartitionTree(tree);
        
        return tree;
    }

    public void RepartitionTree(CachedBubbleChunk tree)
    {
        logger.LogTrace("RepartitionTree(tree={tree})", tree);
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