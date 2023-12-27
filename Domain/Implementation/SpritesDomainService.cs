using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class SpritesDomainService(
    ILogger<SpritesDomainService> logger, 
    PalantirContext db) : ISpritesDomainService
{
    
    public async Task<SpriteEntity> GetSpriteById(int id)
    {
        logger.LogTrace("GetSpriteById(id={id})", id);
        
        var sprite = await db.Sprites.FirstOrDefaultAsync(sprite => sprite.Id == id);
        if (sprite is null)
        {
            throw new EntityNotFoundException($"Sprite with id {id} does not exist.");
        }

        return sprite;
    }

    public async Task<List<SpriteEntity>> GetAllSprites()
    {
        logger.LogTrace("GetAllSprites()");
        
        return await db.Sprites.ToListAsync();
    }
}