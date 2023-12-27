using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class ScenesDomainService(
    ILogger<ScenesDomainService> logger, 
    PalantirContext db) : IScenesDomainService
{
    
    public async Task<SceneEntity> GetSceneById(int id)
    {
        logger.LogTrace("GetSceneByID(id={id})", id);
        
        var scene = await db.Scenes.FirstOrDefaultAsync(scene => scene.Id == id);
        if (scene is null)
        {
            throw new EntityNotFoundException($"Scene with id {id} does not exist.");
        }

        return scene;
    }

    public async Task<List<SceneEntity>> GetAllScenes()
    {
        logger.LogTrace("GetAllScenes()");
        
        return await db.Scenes.ToListAsync();
    }
}