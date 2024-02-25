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
    private static DropChunkLeaf? _leaf; // workaround for singleton-like behavior

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
            var chunkSize = 100000;
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
            DropChunkLeaf? lastLeaf = null;
            for(var i = 0; i < drops.Count; i++)
            {
                var dropStart = drops[i].DropId;
                long? dropEnd = i < drops.Count - 1 ? drops[i + 1].DropId : null;
                lastLeaf = new DropChunkLeaf(services)
                    .WithChunkSize(dropStart, dropEnd);
                tree.AddChunk(lastLeaf);
            }

            _leaf = lastLeaf ?? throw new InvalidOperationException("Tree has no leaves - are there any drops?"); 

            // set tree
            _tree = tree;
            logger.LogDebug("Drop chunk tree finished initialization.");
        }
    }

    public void ReevaluateRange()
    {
        // check if there are new indexes to add chunks
        var chunkSize = 5000;
        var lastChunkStart = _leaf.Chunk.DropIndexStart;
        var drops = _db.PastDrops
            .FromSqlRaw(
                $"SELECT * FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY DropID) AS rn FROM PastDrops WHERE DropID > {lastChunkStart}) hd WHERE (hd.rn % {chunkSize})=0;")
            .ToList();
        
        // if no new indexes found, do not grow tree
        if (drops.Count == 1) return;
        
        // else add new chunks
        _leaf.WithChunkSize(lastChunkStart, drops[2].DropId);
        DropChunkLeaf? lastLeaf = null;
        for(var i = 0; i < drops.Count; i++)
        {
            var dropStart = drops[i].DropId;
            long? dropEnd = i < drops.Count - 1 ? drops[i + 1].DropId : null;
            lastLeaf = new DropChunkLeaf(_services)
                .WithChunkSize(dropStart, dropEnd);
            _tree.AddChunk(lastLeaf);
        }

        _leaf = lastLeaf;
    }
}