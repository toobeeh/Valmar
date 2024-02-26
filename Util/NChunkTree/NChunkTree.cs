using System.Collections;

namespace Valmar.Util.NChunkTree;

/// <summary>
/// A dynamic tree data structure with following characteristics:
/// - each node has a "chunk", which is an abstraction of all its child nodes (or in other words - their chunks)
/// - the leaf node chunks are no abstraction, but the actual data$
/// - a chunk provideds functions that operate on a dataset identified and ordered by strictly monotonic increasing indices
/// - a chunk has an operating range, which is the range of minimum to maximum index of its data
/// - a node can have an arbitrary amount of child nodes
/// - the child nodes in a node are sorted by their non-overlapping range
/// - the index range of a mode is at least the range of smallest to largest index of all its child nodes
/// - if a node is added to a node, it will either be added to the first empty index, or the node with the least amount of child nodes will be expanded
/// - when a node is expanded, all nodes with a index range bigger than the expanded node (ie the nodes right to it) and itself are moved a level further down
///
/// - issues: dynamic sizing of the nodes may cause issues with expanding, when the parent node has more slots than the expanded node
///
/// this structure is used similar as an octree to create abstraction chunks.
/// a node has a chunk data type TChunk which can be used to calculate an abstraction to all its child chunks.
/// the abstraction can implement a caching algorithm using clean/dirty markers and therefor create a big performance boost
/// if the data has large spaces of unchanged data.
/// </summary>
/// <typeparam name="TChunk">The datatype of the chunk that each node holds</typeparam>
public abstract class NChunkTree<TChunk, TProvider> where TProvider : NChunkTreeProvider
{
    public int NodeId => Context.Id;
    
    /// <summary>
    /// The chunk which represents the abstraction of all child nodes' chunks
    /// </summary>
    public abstract TChunk Chunk { get; }
    
    /// <summary>
    /// Reevaluates chunks (child nodes) of the tree; adds or removes chunks depending on the implementation
    /// </summary>
    /// <returns></returns>
    public abstract void RepartitionChunks();
    
    /// <summary>
    ///  The max allowed chunk nodes in this node
    /// </summary>
    protected int NodeCount => Context.ChildIdSlots.Length;
    
    /// <summary>
    /// A list of child node instances; instances are created by provider each request
    /// </summary>
    protected List<NChunkTree<TChunk, TProvider>> Nodes => Provider.GetNodeChildNodes<TChunk, TProvider>(Services, Context.Id);
    
    /// <summary>
    /// The implementation-specific start index of this chunk; null if open
    /// </summary>
    protected abstract long? ChunkStartIndex { get; }
    
    /// <summary>
    /// The implementation-specific end index of this chunk; null if open
    /// </summary>
    protected abstract long? ChunkEndIndex { get; }
    
    /// <summary>
    /// Creates a new node instance with the same chunk type,
    /// used to expand one of the nodes
    /// </summary>
    /// <returns></returns>
    protected abstract NChunkTree<TChunk, TProvider> CreateExpansionNode();
    
    /// <summary>
    /// If this node can take another chunk node
    /// </summary>
    protected bool IsFull => Provider.NodeIsFull(Context.Id);
    
    /// <summary>
    /// The cardinality / recursive amount of chunk nodes in this node
    /// </summary>
    protected int Cardinality => Provider.GetNodeCardinality(Context.Id);
    
    /// <summary>
    /// The level of this node in the tree
    /// </summary>
    protected int Level => Context.Level;
    
    protected readonly NChunkTreeNodeContext Context;
    protected readonly TProvider Provider;
    protected readonly IServiceProvider Services;
    protected NChunkTree(IServiceProvider services, TProvider provider, NChunkTreeNodeContext context)
    {
        Provider = provider;
        Context = context;
        Services = services;
    }

    /// <summary>
    /// Adds a new chunk that will be a leaf.
    /// The chunk MUST start after the biggest existing chunk.
    /// </summary>
    /// <param name="chunk"></param>
    /// <returns>Returns this node for fluent interface</returns>
    public virtual NChunkTree<TChunk, TProvider> AddChunk(NChunkTree<TChunk, TProvider> chunk)
    {
        // find node position that is either not set or can take chunks in it
        var freeSlot = -1;
        var slots = Context.ChildIdSlots;
        for(int i = 0; i < NodeCount; i++)
        {
            if (slots[i] is {} nodeId && Provider.NodeIsFull(nodeId)) continue;
            
            // free node found
            freeSlot = i;
            break;
        }
        
        // ensure that new chunk starts after the all chunks currently existing
        if(ChunkEndIndex > chunk.ChunkStartIndex)
        {
            throw new ArgumentException("New chunk must start after the biggest existing chunk");
        }
        
        // expand this node if all chunk nodes are full
        if (freeSlot == -1)
        {
            // find node with least cardinality to be expanded
            var expansionIndex = slots
                .Select((node, index) => new {count = Provider.GetNodeCardinality(node ?? throw new NullReferenceException("Node id was null")), index})
                .MinBy(item => item.count)!
                .index;
            
            // move existing chunk nodes starting from expansion index to new chunk node
            var expansion = CreateExpansionNode();
            for(var i = expansionIndex; i < NodeCount; i++)
            {
                var node = Provider.GetNode<TChunk, TProvider>(Services, slots[i] ?? throw new NullReferenceException("Node id was null"));
                expansion.AddChunk(node);
                slots[i] = null;
            }
            
            // set expansion chunk node to expansion index, and execute add chunk again (now should be free space!)
            slots[expansionIndex] = expansion.NodeId;
            AddChunk(chunk);
        }

        // add chunk to next index
        else
        {
            if (slots[freeSlot] is {} freeNodeId)
            {
                var node = Provider.GetNode<TChunk, TProvider>(Services, freeNodeId);
                node.AddChunk(chunk);
            }
            else
            {
                slots[freeSlot] = chunk.NodeId;
            }
        }

        return this;
    }
}