using Valmar.Database;

namespace Valmar.Domain;

public interface IScenesDomainService
{
    Task<SceneEntity> GetSceneById(int id);
    Task<List<SceneEntity>> GetAllScenes();
}