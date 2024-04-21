using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface ISpritesDomainService
{
    Task<SpriteDdo> GetSpriteById(int id);
    Task<List<SpriteDdo>> GetAllSprites(int? eventId = null);
    Task<List<SpriteRankingDdo>> GetSpriteRanking();
}