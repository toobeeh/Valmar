using Valmar.Database;

namespace Valmar.Domain;

public interface ISpritesDomainService
{
    Task<SpriteEntity> GetSpriteById(int id);
    Task<List<SpriteEntity>> GetAllSprites();
}