using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IScenesDomainService
{
    Task<SceneEntity> GetSceneById(int id);
    Task<List<SceneEntity>> GetAllScenes();
    Task<List<SceneRankingDdo>> GetSceneRanking();
    Task<SceneEntity> GetSceneByEventId(int id);
    Task<List<SceneThemeEntity>> GetAllSceneThemes();
    Task<List<SceneThemeEntity>> GetThemesOfScene(int sceneId);
    Task<SceneEntity> AddScene(string name, string url, string? artist, bool exclusive, int? eventId);

    Task<SceneEntity> UpdateScene(int id, string name, string url, string? artist, bool exclusive,
        int? eventId);
}