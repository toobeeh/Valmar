using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class ScenesDomainService(
    ILogger<ScenesDomainService> logger,
    PalantirContext db) : IScenesDomainService
{
    public async Task<SceneEntity> GetSceneById(int id)
    {
        logger.LogTrace("GetSceneById(id={id})", id);

        var scene = await db.Scenes.FirstOrDefaultAsync(scene => scene.Id == id);
        if (scene is null)
        {
            throw new EntityNotFoundException($"Scene with id {id} does not exist.");
        }

        return scene;
    }

    public async Task<SceneEntity> GetSceneByEventId(int id)
    {
        logger.LogTrace("GetSceneByEventId(id={id})", id);

        var scene = await db.Scenes.FirstOrDefaultAsync(scene => scene.EventId == id);
        if (scene is null)
        {
            throw new EntityNotFoundException($"Scene for event id {id} does not exist.");
        }

        return scene;
    }

    public async Task<List<SceneEntity>> GetAllScenes()
    {
        logger.LogTrace("GetAllScenes()");

        return await db.Scenes.ToListAsync();
    }

    public async Task<List<SceneThemeEntity>> GetAllSceneThemes()
    {
        logger.LogTrace("GetAllSceneThemes()");

        return await db.SceneThemes.ToListAsync();
    }

    public async Task<List<SceneThemeEntity>> GetThemesOfScene(int sceneId)
    {
        logger.LogTrace("GetSceneThemesOfScene(sceneId={sceneId})", sceneId);

        var sceneThemes = await db.SceneThemes
            .Where(theme => theme.SceneId == sceneId)
            .ToListAsync();
        return sceneThemes;
    }

    public async Task<List<SceneRankingDdo>> GetSceneRanking()
    {
        logger.LogTrace("GetSpriteRanking()");

        var scenes = await db.Scenes.Select(sprite => sprite.Id).ToListAsync();
        var inventories = await db.Members.Select(member => member.Scenes).Where(inv => !string.IsNullOrWhiteSpace(inv))
            .ToListAsync();
        var parsedInventories = inventories
            .Select(inv => inv!
                .Split(",")
                .Select(id => new
                    { Id = Convert.ToInt32(id.Split(":")[0].Replace(".", "")), Active = id.Contains('.') }).ToList())
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

    public async Task<SceneEntity> AddScene(string name, string url, string? artist, bool exclusive, int? eventId)
    {
        logger.LogTrace("AddScene(name={name}, url={url}, artist={artist}, exclusive={exclusive}, eventId={eventId})",
            name, url, artist, exclusive, eventId);

        var scene = new SceneEntity
        {
            Name = name,
            Url = url,
            Artist = artist ?? "",
            Exclusive = exclusive,
            EventId = eventId ?? 0
        };

        var entity = db.Scenes.Add(scene);
        await db.SaveChangesAsync();

        return entity.Entity;
    }

    public async Task<SceneEntity> UpdateScene(int id, string name, string url, string? artist, bool exclusive,
        int? eventId)
    {
        logger.LogTrace(
            "UpdateScene(id={id}, name={name}, url={url}, artist={artist}, exclusive={exclusive}, eventId={eventId})",
            id, name, url, artist, exclusive, eventId);

        var scene = await GetSceneById(id);
        scene.Name = name;
        scene.Url = url;
        scene.Artist = artist ?? "";
        scene.Exclusive = exclusive;
        scene.EventId = eventId ?? 0;

        db.Scenes.Update(scene);
        await db.SaveChangesAsync();

        return scene;
    }
}