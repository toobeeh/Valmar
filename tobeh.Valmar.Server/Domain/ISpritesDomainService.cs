using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface ISpritesDomainService
{
    Task<SpriteDdo> GetSpriteById(int id);
    Task<List<SpriteDdo>> GetAllSprites(int? eventId = null);
    Task<List<SpriteRankingDdo>> GetSpriteRanking();

    Task<int> AddSprite(string name, string url, int cost, int? eventDropId,
        string? artist, bool rainbow);
}