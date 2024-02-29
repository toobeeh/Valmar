using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Exceptions;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

public class AdminDomainService(
    ILogger<AdminDomainService> logger, 
    DropChunkTreeProvider dropChunks) : IAdminDomainService
{
    
    public async Task ReevaluateDropChunks()
    {
        logger.LogTrace("ReevaluateDropChunks()");

        var tree = dropChunks.GetTree();
        dropChunks.RepartitionTree(tree);
    }
}