using Valmar.Database;

namespace Valmar.Domain;

public interface IAwardsDomainService
{
    Task<AwardEntity> GetAwardById(int id);
    Task<List<AwardEntity>> GetAllAwards();
}