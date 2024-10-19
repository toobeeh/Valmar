using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Domain;

public interface IAwardsDomainService
{
    Task<AwardEntity> GetAwardById(int id);
    Task<List<AwardEntity>> GetAllAwards();
    Task<AwardEntity> CreateAward(string name, int rarity, string url, string description);
}