using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.Param;
using Valmar.Domain.Exceptions;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

public class InventoryDomainService(
    ILogger<InventoryDomainService> logger, 
    PalantirContext db,
    IMembersDomainService membersService,
    ISpritesDomainService spritesService,
    IEventsDomainService eventsService,
    DropChunkTreeProvider dropChunks) : IInventoryDomainService
{
    public async Task<List<MemberSpriteSlotDdo>> GetMemberSpriteInventory(int login)
    {
        logger.LogTrace("GetMemberSpriteInventory(login={login})", login);
        
        var member = await membersService.GetMemberByLogin(login);
        var inv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites);
        return inv;
    }
    
    public async Task<BubbleCreditDdo> GetBubbleCredit(int login)
    {
        logger.LogTrace("GetBubbleCredit(login={login})", login);
        
        var member = await membersService.GetMemberByLogin(login);
        
        var chunk = dropChunks.GetTree().Chunk;
        var leagueDrops = (int)Math.Floor(await chunk.GetLeagueWeight(member.DiscordId.ToString()));

        var spriteInv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites).Select(inv => inv.SpriteId).ToArray();
        var invSum = await db.Sprites.Where(sprite => sprite.EventDropId == 0 && spriteInv.Contains(sprite.Id))
            .SumAsync(sprite => sprite.Cost);

        var sceneInv = InventoryHelper.ParseSceneInventory(member.Scenes).ToArray();
        var nonEventScenes = await db.Scenes.Where(scene => scene.EventId == 0 && !scene.Exclusive && sceneInv.Contains(scene.Id)).ToListAsync(); 
        var sceneSum = nonEventScenes.Select((scene, index) => SceneHelper.GetScenePrice(index)).Sum();

        var totalBubbles = member.Bubbles + (member.Drops + leagueDrops) * 50;
        var availableBubbles = totalBubbles - invSum - sceneSum;
        
        return new BubbleCreditDdo(totalBubbles, availableBubbles, member.Bubbles);
    }
    
    public async Task<List<EventCreditDdo>> GetEventCredit(int login, List<int> eventDropIds)
    {
        logger.LogTrace("GetEventCredit(login={login})", login);
        
        var inv = (await GetMemberSpriteInventory(login)).Select(inv => inv.SpriteId).ToArray();
        
        var spentCredits = await db.Sprites
            .Where(sprite => eventDropIds.Contains(sprite.EventDropId) && inv.Contains(sprite.Id))
            .GroupBy(sprite => sprite.EventDropId)
            .Select(group => new { EventDropId = group.Key, Cost = group.Sum(sprite => sprite.Cost) })
            .ToListAsync();
        
        var credits = await db.EventCredits
            .Where(credit => credit.Login == login && eventDropIds.Contains(credit.EventDropId))
            .ToListAsync();

        var creditResults = eventDropIds.Select(dropId =>
        {
            var totalCredit = credits.FirstOrDefault(credit => credit.EventDropId == dropId)?.Credit ?? 0;
            var availableCredit = totalCredit - spentCredits.FirstOrDefault(spent => spent.EventDropId == dropId)?.Cost ?? 0;
            return new EventCreditDdo(dropId, totalCredit, availableCredit);
        }).ToList();
        
        return creditResults;
    }

    public async Task BuySprite(int login, int spriteId)
    {
        logger.LogTrace("BuySprite(login={login}, spriteId={spriteId})", login, spriteId);
        
        // check if the sprite is already in the inventory
        var inv = await GetMemberSpriteInventory(login);
        if (inv.Any(slot => slot.SpriteId == spriteId))
        {
            throw new UserOperationException("The sprite is already in the inventory");
        }
        
        // check if the sprite is available for purchase
        var sprite = await spritesService.GetSpriteById(spriteId);
        if (sprite.Released == false)
            throw new UserOperationException($"The event sprite {sprite.Name} ({sprite.Id}) is not released yet");
        
        if (sprite.EventDropId > 0)
        {
            var credit = (await GetEventCredit(login, [sprite.EventDropId]))
                .FirstOrDefault(credit => credit.EventDropId == sprite.EventDropId)?.AvailableCredit ?? 0;
            if(credit < sprite.Cost) throw new UserOperationException($"The sprite price ({sprite.Cost}) exceeds the available event credit ({credit}) of the event drop {sprite.Name} ({sprite.EventDropId})");
        }
        else
        {
            var credit = await GetBubbleCredit(login);
            if(credit.AvailableCredit < sprite.Cost) throw new UserOperationException($"The sprite price ({sprite.Cost}) exceeds the available bubble credit ({credit.AvailableCredit})");
        }
        
        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login) ?? throw new EntityNotFoundException("The sprite can't be added to member inventory, because member doesn't exist");
        inv.Add(new MemberSpriteSlotDdo(0, sprite.Id, null));
        member.Sprites = InventoryHelper.SerializeSpriteInventory(inv);
        db.Members.Update(member);
        await db.SaveChangesAsync();
    }
}