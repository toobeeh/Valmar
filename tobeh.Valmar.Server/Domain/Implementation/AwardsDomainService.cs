using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Exceptions;

namespace tobeh.Valmar.Server.Domain.Implementation;

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

    public async Task<AwardEntity> CreateAward(string name, int rarity, string url, string description)
    {
        logger.LogTrace("CreateAward(name={name}, rarity={rarity}, url={url}, description={description})", name, rarity,
            url, description);

        var award = new AwardEntity
        {
            Name = name,
            Rarity = (sbyte)rarity,
            Url = url,
            Description = description
        };
        db.Awards.Add(award);
        await db.SaveChangesAsync();

        return award;
    }
}