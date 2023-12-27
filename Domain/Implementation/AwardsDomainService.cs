using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class AwardsDomainService(
    ILogger<AwardsDomainService> logger, 
    PalantirContext db) : IAwardsDomainService
{
    
    public async Task<AwardEntity> GetAwardById(int id)
    {
        logger.LogTrace("GetAwardById(id={id})", id);
        
        var award = await db.Awards.FirstOrDefaultAsync(award => award.Id == id);
        if (award is null)
        {
            throw new EntityNotFoundException($"Award with id {id} does not exist");
        }

        return award;
    }

    public async Task<List<AwardEntity>> GetAllAwards()
    {
        logger.LogTrace("GetAllAwards()");
        return await db.Awards.ToListAsync();
    }
}