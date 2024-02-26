using System.Collections.Concurrent;

namespace Valmar.Util.NChunkTree;

public record NChunkTreeNodeContext(int Id, int?[] ChildIdSlots, int Level, Type NodeType);

public abstract class NChunkTreeProvider
{
    private int _nextId = 0;
    private readonly object _nodeCreateLock = new();
    private readonly ConcurrentDictionary<int, NChunkTreeNodeContext> _nodes = new();
    
    public NChunkTree<TChunk, TProvider> GetNode<TChunk, TProvider>(IServiceProvider provider, int id) 
        where TProvider : NChunkTreeProvider
    {
        if (!_nodes.TryGetValue(id, out var context)) throw new InvalidOperationException("Node not found");
        
        var tree = ActivatorUtilities.CreateInstance(provider, context.NodeType, context) as NChunkTree<TChunk, TProvider> ?? throw new NullReferenceException("Could not instantiate node") ;
        return tree;
    }

    public TNode CreateNode<TChunk, TProvider, TNode>(IServiceProvider provider, int nodeCount, int level) 
        where TNode : NChunkTree<TChunk, TProvider> 
        where TProvider : NChunkTreeProvider
    {
        
        if(! typeof(TProvider).IsAssignableTo(GetType())) throw new InvalidOperationException($"Tried to access from foreign provider implementation {typeof(TProvider).FullName}");

        // ensure thread-safety
        int id;
        lock (_nodeCreateLock)
        {
            id = _nextId++;
        }
        
        var context = new NChunkTreeNodeContext(id, new int?[nodeCount], level, typeof(TNode));
        _nodes.TryAdd(context.Id, context);
        
        var tree = ActivatorUtilities.CreateInstance<TNode>(provider, context) ?? throw new NullReferenceException("Could not instantiate node");
        return tree;
    }

    public List<NChunkTree<TChunk, TProvider>> GetNodeChildNodes<TChunk, TProvider>(IServiceProvider provider, int id) where TProvider : NChunkTreeProvider
    {
        if (!_nodes.TryGetValue(id, out var context)) throw new InvalidOperationException("Node not found");
        var nodes = context.ChildIdSlots
            .Where(id => id.HasValue)
            .Select(id => GetNode<TChunk, TProvider>(provider, id.Value));

        return nodes.ToList();
    }

    public int GetNodeCardinality(int id)
    {
        var node = _nodes[id];
        return 1 + node.ChildIdSlots
            .Select(childId => childId is {} childIdValue ? GetNodeCardinality(childIdValue) : 0)
            .Sum();
    }

    public bool NodeIsFull(int id)
    {
        var node = _nodes[id];
        return node.ChildIdSlots.All(childId => childId is {} childIdValue && NodeIsFull(childIdValue));
    }

    public virtual void RemoveNode(int id, int? parentId)
    {
        // remove from parent
        if (parentId is {} parentIdValue)
        {
            if (!_nodes.TryGetValue(parentIdValue, out var parentContext)) throw new InvalidOperationException("Parent Node not found");
            var index = parentContext.ChildIdSlots.ToList().FindIndex(item => item == id);
            parentContext.ChildIdSlots[index] = null;
            
            // must remove following nodes too to maintain integrity
            // TODO
        }
        
        // remove node
        if (!_nodes.TryGetValue(id, out var context)) throw new InvalidOperationException("Node not found");
        _nodes.TryRemove(id, out _);
        
        // remove children
        foreach (var childIdSlot in context.ChildIdSlots)
        {
            if (childIdSlot is {} childId) RemoveNode(childId, null); // dont remove from parent, has already been removed
        }
    }
}