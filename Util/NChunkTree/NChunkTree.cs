using System.Collections;

namespace Valmar.Util.NChunkTree;

/// <summary>
/// A dynamic tree data structure with following characteristics:
/// - A node can have an arbitrary amount of child nodes
/// - If a node has 0 child nodes, it's a leaf
/// - each node has a range of indices
/// - the child nodes in a node are sorted by their non-overlapping range
/// - the index range of a node can be set by the implementation, but all child nodes must be in that range
/// - if a node is added to a node, it will either be added to the first empty index, or the node with the least amount of child nodes will be expanded
/// - when a node is expanded, all nodes to its right will be moved to the new node
///
/// - issues: dynamic sizing of the nodes may cause issues with expanding, when the parent node has more slots than the expanded node
///
/// this strucutre is used similar as an octree to create abstraction chunks.
/// a node has a chunk data type TChunk which can be used to calculate an abstraction to all its child chunks.
/// the abstraction can implement a caching algorithm using clean/dirty markers and therefor create a big performance boost
/// if the data has large spaces of unchanged data.
/// </summary>
/// <typeparam name="TChunk"></typeparam>
public abstract class NChunkTree<TChunk>
{
    private static int _nextId = 0;
    
    private readonly int _id = NChunkTree<TChunk>._nextId++;
    
    /// <summary>
    ///  The max allowed chunk nodes in this node
    /// </summary>
    protected readonly int ChunkNumber;
    
    /// <summary>
    /// If this node can take another chunk node
    /// </summary>
    private bool IsFull { get {
        for (var i = 0; i < ChunkNumber; i++)
        {
            if (_chunks[i] == null || !_chunks[i]!.IsFull) return false;
        }
        return true;
    }}
    
    /// <summary>
    /// The cardinality / recursive amount of chunk nodes in this node
    /// </summary>
    private int Cardinality { get
    {
        var count = 1; // this chunk
        for (var i = 0; i < _nextUnsetChunkIndex; i++)
        {
            count += _chunks[i]!.Cardinality; // chunks in chunk nodes
        }
        return count;
    }}
    
    /// <summary>
    /// The implementation-specific start index of this chunk
    /// </summary>
    protected abstract long ChunkStartIndex { get; }
    
    /// <summary>
    /// The implementation-specific end index of this chunk
    /// </summary>
    protected abstract long ChunkEndIndex { get; }
    
    /// <summary>
    /// Creates a new instance of the same chunk node type,
    /// used to expand one of the nodes
    /// </summary>
    /// <returns></returns>
    protected abstract NChunkTree<TChunk> CreateExpansionNode();
    
    /// <summary>
    /// Gets the chunk abstraction that is stored in this node
    /// </summary>
    protected abstract TChunk Chunk { get; }

    private readonly NChunkTree<TChunk>?[] _chunks;
    private int _nextUnsetChunkIndex = 0;
    
    protected NChunkTree(int chunkCount)
    {
        ChunkNumber = chunkCount;
        _chunks = new NChunkTree<TChunk>?[chunkCount];
    }

    /// <summary>
    /// Adds a new chunk that will be a leaf.
    /// The chunk MUST start after the biggest existing chunk.
    /// </summary>
    /// <param name="chunk"></param>
    /// <returns>Returns this node for fluent interface</returns>
    public NChunkTree<TChunk> AddChunk(NChunkTree<TChunk> chunk)
    {
        
        // find node position that is either not set or can take chunks in it
        var freeChunk = -1;
        for(int i = 0; i < ChunkNumber; i++)
        {
            if (_chunks[i] == null || !_chunks[i]!.IsFull)
            {
                freeChunk = i;
                break;
            }
        }
        
        // ensure that new chunk starts after the biggest existing chunk of the stored nodes
        if(_chunks[freeChunk == -1 ? ChunkNumber - 1 : freeChunk]?.ChunkEndIndex > chunk.ChunkStartIndex)
        {
            throw new ArgumentException("New chunk must start after the biggest existing chunk");
        }
        
        // expand this node if all chunk nodes are full
        if (freeChunk == -1)
        {
            // find node with least cardinality to be expanded
            var leastCardinality = int.MaxValue;
            var expansionIndex = 0;
            for(var i = 0; i < ChunkNumber; i++)
            {
                var cardinality = _chunks[i]!.Cardinality;
                if (cardinality < leastCardinality)
                {
                    leastCardinality = cardinality;
                    expansionIndex = i;
                }
            }
            
            // move existing chunk nodes starting from expansion index to new chunk node
            var expansion = CreateExpansionNode();
            for(var i = expansionIndex; i < ChunkNumber; i++)
            {
                expansion.AddChunk(_chunks[i]!);
                _chunks[i] = null;
            }
            
            // set expansion chunk node to expansion index, and execute add chunk again (now should be free space!)
            _chunks[expansionIndex] = expansion;
            _nextUnsetChunkIndex = expansionIndex + 1;
            AddChunk(chunk);
        }

        // add chunk to next index
        else
        {
            if (_chunks[freeChunk] == null)
            {
                _chunks[freeChunk] = chunk;
                _nextUnsetChunkIndex++;
            }
            else
            {
                _chunks[freeChunk]!.AddChunk(chunk);
            }
        }

        return this;
    }

    public IEnumerable<TChunk> Chunks => new ChunkEnumerable(this);

    private class ChunkEnumerable(NChunkTree<TChunk> tree) : IEnumerable<TChunk>
    {
        public IEnumerator<TChunk> GetEnumerator()
        {
            return new ChunkEnumerator(tree);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private class ChunkEnumerator(NChunkTree<TChunk> tree) : IEnumerator<TChunk>
    {
        private int _index = -1;

        public bool MoveNext()
        {
            return ++_index < tree._nextUnsetChunkIndex;
        }

        public void Reset()
        {
            _index = -1;
        }

        public TChunk Current => tree._chunks[_index]!.Chunk;

        object IEnumerator.Current => Current!;

        public void Dispose()
        {
            // pass
        }
    }
}