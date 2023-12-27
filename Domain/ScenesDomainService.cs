using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Grpc;

namespace Valmar.Domain;

public class ScenesDomainService(
    ILogger<ScenesGrpcService> logger, 
    PalantirContext db)
{
    
    public async Task<SceneEntity?> GetSceneById(int id)
    {
        logger.LogTrace("GetSceneByID(id={id})", id);
        return await db.Scenes.FirstOrDefaultAsync(scene => scene.Id == id);
    }

    public async Task<List<SceneEntity>> GetAllScenes()
    {
        logger.LogTrace("GetAllScenes()");
        return await db.Scenes.ToListAsync();
    }
}