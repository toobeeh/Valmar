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
}