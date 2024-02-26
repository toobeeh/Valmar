using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Util.NChunkTree;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation.Drops;

/// <summary>
/// A wrapper around the drop chunk caching system;
/// needs palantirdb service as transient and persistentdropchunk in the service collection
/// </summary>
public class DropCache
{
    private readonly PalantirContext _db;
    private readonly IServiceProvider _services;
    private static IDropChunk? _tree; // workaround for singleton-like behavior
    private static PersistentDropChunk? _leaf; // workaround for singleton-like behavior

    public IDropChunk Drops
    {
        get
        {
            if (_tree is null) throw new NullReferenceException("Tree has not been initialized");
            return _tree;
        }
    }

    public DropCache(PalantirContext db, IServiceProvider services, ILogger<DropCache> logger, DropChunkTreeProvider provider)
    {
        _services = services;
        _db = db;
        
        // init tree if not yet done
        if (_tree is null)
        {
            var chunkSize = 5000;
            var treeBranchingCoeff = 2;
        
            logger.LogDebug("Indexing drops for chunking...");
        
            // find indexes to index chunks
            var drops = db.PastDrops
                .OrderBy(d => d.DropId)
                .AsEnumerable()
                .Where((drop, index) => (index) % chunkSize == 0)
                .ToList();
        
            // create chunks
            logger.LogDebug("Building chunk tree...");
            var tree = provider.GetTree(services);
            
            PersistentDropChunk? lastLeaf = null;
            for(var i = 0; i < drops.Count; i++)
            {
                var dropStart = drops[i].DropId;
                long? dropEnd = i < drops.Count - 1 ? drops[i + 1].DropId : null;

                lastLeaf = provider.CreateLeaf(services, dropStart, dropEnd);
                tree.AddChunk(lastLeaf);
            }

            _leaf = lastLeaf ?? throw new InvalidOperationException("Tree has no leaves - are there any drops?"); 

            // set tree
            _tree = tree;
            logger.LogDebug("Drop chunk tree finished initialization.");
        }
    }
}