using Microsoft.EntityFrameworkCore;
using Valmar.Database;
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
    private static DropChunkTree? _tree; // workaround for singleton-like behavior

    public IDropChunk Drops
    {
        get
        {
            if (_tree is null) throw new NullReferenceException("Tree has not been initialized");
            return _tree.Chunk;
        }
    }

    public DropCache(PalantirContext db, IServiceProvider services, ILogger<DropCache> logger)
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
                .FromSqlRaw(
                    $"SELECT * FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY DropID) AS rn FROM PastDrops) hd WHERE (hd.rn % {chunkSize})=0;")
                .ToList();
        
            // create chunks
            logger.LogDebug("Building chunk tree...");
            var tree = new DropChunkTree(services, treeBranchingCoeff);
            for(int i = 0; i < drops.Count; i++)
            {
                var dropStart = drops[i].DropId;
                long? dropEnd = i < drops.Count() - 1 ? drops[i + 1].DropId : null;
                var leaf = new DropChunkLeaf(services)
                    .WithChunkSize(dropStart, dropEnd);
                tree.AddChunk(leaf);
            }

            // set tree
            _tree = tree;
            logger.LogDebug("Drop chunk tree finished initialization.");
        }
    }
}