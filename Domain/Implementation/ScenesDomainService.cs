using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
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
    
    public async Task<List<SceneRankingDdo>> GetSpriteRanking()
    {
        logger.LogTrace("GetSpriteRanking()");

        var scenes = await db.Scenes.Select(sprite => sprite.Id).ToListAsync();
        var inventories = await db.Members.Select(member => member.Scenes).Where(inv => !string.IsNullOrWhiteSpace(inv)).ToListAsync();
        var parsedInventories = inventories
            .Select(inv => inv
                .Split(",")
                .Select(id => new { Id = Convert.ToInt32(id.Replace(".", "")), Active =  id.Contains('.')}).ToList())
            .ToList();

        var sceneRanking = scenes.Select(scene =>
            {
                var bought = parsedInventories.Count(inv => inv.Any(invScene => invScene.Id == scene));
                var active = parsedInventories.Count(inv =>
                    inv.Any(invScene => invScene.Id == scene && invScene.Active));
                return new SceneRankingDdo(scene, active, bought, active * 10 + bought);
            }
        ).ToList();
        
        // map score property of record to index of ranking
        var orderedRanking = sceneRanking
            .OrderByDescending(scene => scene.Rank)
            .Select((scene, index) => scene with { Rank = index + 1 })
            .ToList();
        
        return orderedRanking;
    }
}