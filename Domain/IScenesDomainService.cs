using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IScenesDomainService
{
    Task<SceneEntity> GetSceneById(int id);
    Task<List<SceneEntity>> GetAllScenes();
    Task<List<SceneRankingDdo>> GetSpriteRanking();
}