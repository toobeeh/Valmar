using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface ISpritesDomainService
{
    Task<SpriteDdo> GetSpriteById(int id);
    Task<List<SpriteDdo>> GetAllSprites();
    Task<List<SpriteRankingDdo>> GetSpriteRanking();
}