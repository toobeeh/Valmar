using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Exceptions;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

public class InventoryDomainService(
    ILogger<InventoryDomainService> logger, 
    PalantirContext db,
    IMembersDomainService membersService,
    ISpritesDomainService spritesService,
    IScenesDomainService scenesService,
    IEventsDomainService eventsService,
    IStatsDomainService statsService,
    DropChunkTreeProvider dropChunks) : IInventoryDomainService
{
    public async Task<List<MemberSpriteSlotDdo>> GetMemberSpriteInventory(int login)
    {
        logger.LogTrace("GetMemberSpriteInventory(login={login})", login);
        
        var member = await membersService.GetMemberByLogin(login);
        var inv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites);
        return inv;
    }
    
    public async Task<SceneInventoryDdo> GetMemberSceneInventory(int login)
    {
        logger.LogTrace("GetMemberSceneInventory(login={login})", login);
        
        var member = await membersService.GetMemberByLogin(login);
        var inv = InventoryHelper.ParseSceneInventory(member.Scenes);
        return inv;
    }
    
    public async Task<BubbleCreditDdo> GetBubbleCredit(int login, DropCreditDdo dropCredit)
    {
        logger.LogTrace("GetBubbleCredit(login={login})", login);
        
        var member = await membersService.GetMemberByLogin(login);

        var spriteInv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites).Select(inv => inv.SpriteId).ToArray();
        var invSum = await db.Sprites.Where(sprite => sprite.EventDropId == 0 && spriteInv.Contains(sprite.Id))
            .SumAsync(sprite => sprite.Cost);

        var sceneInv = InventoryHelper.ParseSceneInventory(member.Scenes).SceneIds.ToArray();
        var nonEventScenes = await db.Scenes.Where(scene => scene.EventId == 0 && !scene.Exclusive && sceneInv.Contains(scene.Id)).ToListAsync(); 
        var sceneSum = nonEventScenes.Select((scene, index) => SceneHelper.GetScenePrice(index)).Sum();

        var totalBubbles = member.Bubbles + dropCredit.TotalCredit * 50;
        var availableBubbles = totalBubbles - invSum - sceneSum;
        
        return new BubbleCreditDdo(totalBubbles, availableBubbles, member.Bubbles);
    }
    
    public async Task<DropCreditDdo> GetDropCredit(int login)
    {
        logger.LogTrace("GetDropCredit(login={login})", login);
        
        var member = await membersService.GetMemberByLogin(login);
        
        var chunk = dropChunks.GetTree().Chunk;
        var leagueDrops = (int)Math.Floor(await chunk.GetLeagueWeight(member.DiscordId.ToString()));
        var leagueCount = await chunk.GetLeagueCount(member.DiscordId.ToString());
        
        return new DropCreditDdo(member.Drops + leagueDrops, member.Drops, leagueCount);
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
            var dropCredit = await GetDropCredit(login);
            var credit = await GetBubbleCredit(login, dropCredit);
            if(credit.AvailableCredit < sprite.Cost) throw new UserOperationException($"The sprite price ({sprite.Cost}) exceeds the available bubble credit ({credit.AvailableCredit})");
        }
        
        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login) ?? throw new EntityNotFoundException("The sprite can't be added to member inventory, because member doesn't exist");
        inv.Add(new MemberSpriteSlotDdo(0, sprite.Id, null));
        member.Sprites = InventoryHelper.SerializeSpriteInventory(inv);
        db.Members.Update(member);
        await db.SaveChangesAsync();
    }

    public async Task<int> GetSpriteSlotCount(int login)
    {
        logger.LogTrace("GetSpriteSlotCount(login={login})", login);

        var member = await membersService.GetMemberByLogin(login);
        var isPatron = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Patron);
        var isAdmin = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Admin);
        var dropCredit = await GetDropCredit(login);
        
        var slots = 1 + (isPatron ? 1 : 0) + (isAdmin ? 100 : 0) + dropCredit.TotalCredit / 1000;
        return slots;
    }

    public async Task SetColorShiftConfiguration(int login, Dictionary<int, int?> colorShiftMap, bool clearOther = false)
    {
        logger.LogTrace("SetColorShiftConfiguration(login={login}, colorShiftMap={colorShiftMap}, clearOther={clearOther})", login, colorShiftMap, clearOther);

        var member = await membersService.GetMemberByLogin(login);
        var inv = await GetMemberSpriteInventory(login);
        var isPatron = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Patron);
        var isAdmin = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Admin);

        // if clearing, remove all configs
        if (!clearOther)
        {
            inv = inv.Select(slot => slot with { ColorShift = null }).ToList();
        }
        
        // set new configs 
        foreach (var (spriteId, colorShift) in colorShiftMap)
        {
            var slotIndex = inv.FindIndex(slot => slot.SpriteId == spriteId);
            if (slotIndex != -1) inv[slotIndex] = inv[slotIndex] with { ColorShift = colorShift };
            else throw new UserOperationException($"The user does not own the sprite {spriteId}");
        }
        
        // check if user is allowed to set more than one color shifts
        if (!isPatron && !isAdmin && inv.Count(slot => slot.ColorShift != null) > 1)
        {
            throw new UserOperationException("User is not allowed to set more than one color shift");
        }
        
        var configString = String.Join(",", inv
            .Where(slot => slot.ColorShift != null)
            .Select(slot => $"{slot.SpriteId}:{slot.ColorShift}"));
        
        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == login) ?? throw new EntityNotFoundException("The color shift configuration can't be saved because member doesn't exist");
        memberEntity.RainbowSprites = configString;
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }
    
    public async Task UseScene(int login, int? sceneId)
    {
        logger.LogTrace("UseScene(login={login}, sceneId={sceneId})", login, sceneId);
        
        var member = await membersService.GetMemberByLogin(login);
        var inv = InventoryHelper.ParseSceneInventory(member.Scenes);
        
        // check if user owns the scene 
        if (sceneId is {} sceneIdValue && !inv.SceneIds.Contains(sceneIdValue))
        {
            throw new UserOperationException($"The user does not own the scene {sceneIdValue}");
        }
        
        // set new active scene
        inv = inv with { ActiveId = sceneId };
        
        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == login) ?? throw new EntityNotFoundException("The scene can't be activated because member doesn't exist");
        memberEntity.Scenes = InventoryHelper.SerializeSceneInventory(inv);
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }
    
    public async Task BuyScene(int login, int sceneId)
    {
        logger.LogTrace("BuyScene(login={login}, sceneId={sceneId})", login, sceneId);
        
        var member = await membersService.GetMemberByLogin(login);
        var inv = InventoryHelper.ParseSceneInventory(member.Scenes);
        
        // check if user owns the scene 
        if (inv.SceneIds.Contains(sceneId))
        {
            throw new UserOperationException($"The user already owns the scene {sceneId}");
        }
        
        // check if the scene is available for purchase
        var scene = await scenesService.GetSceneById(sceneId);
        if(scene.Exclusive)
        {
            throw new UserOperationException(
                $"The scene {scene.Name} ({scene.Id}) is exclusive and can't be purchased");
        }
        
        // check if scene is event scene or regular scene and verify price
        if (scene.EventId != 0)
        {
            var sceneEvent = await eventsService.GetEventById(scene.EventId);
            var eventPrice = EventHelper.GetEventScenePrice(sceneEvent.Length);
            var traceBeforeStart = sceneEvent.StartDate.AddDays(-1); // traces are record from end of day - event start credit is from day before start
            var eventBubblesCollected = await statsService.GetMemberBubblesInRange(login, traceBeforeStart, sceneEvent.EndDate);
            var totalCollectedDuringEvent = eventBubblesCollected.EndAmount - eventBubblesCollected.StartAmount;
            
            if(eventPrice > totalCollectedDuringEvent)
            {
                throw new UserOperationException(
                    $"The scene price ({eventPrice}) exceeds the available bubbles ({totalCollectedDuringEvent})");
            }
        }
        else
        {
            var dropCredit = await GetDropCredit(login);
            var bubbleCredit = await GetBubbleCredit(login, dropCredit);
            var scenePrice = SceneHelper.GetScenePrice(inv.SceneIds.Count);
            
            if(scenePrice > bubbleCredit.AvailableCredit)
            {
                throw new UserOperationException(
                    $"The scene price ({scenePrice}) exceeds the available bubble credit ({bubbleCredit.AvailableCredit})");
            }
        }
        
        // add scene to inventory
        inv.SceneIds.Add(scene.Id);
        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == login) ?? throw new EntityNotFoundException("The scene can't be activated because member doesn't exist");
        memberEntity.Scenes = InventoryHelper.SerializeSceneInventory(inv);
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }

    public async Task UseSpriteCombo(int login, List<int> combo, bool clearOther = false)
    {
        logger.LogTrace("UseSpriteCombo(login={login}, combo={combo}, clearOther={clearOther})", login, combo, clearOther);
        
        var member = await membersService.GetMemberByLogin(login);
        var inv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites);
        
        // check if all sprites are in the inventory
        if (combo.Any(id => inv.All(slot => slot.SpriteId != id)))
        {
            throw new UserOperationException($"The user does not own all sprites from the combo {combo}");
        }
        
        // check if all sprites are unique
        if (combo.Distinct().Count() != combo.Count)
        {
            throw new UserOperationException("The combo contains duplicate sprites");
        }
        
        // check if user has enough sprite slots
        if (combo.Count > await GetSpriteSlotCount(login))
        {
            throw new UserOperationException("The user does not have enough sprite slots to activate the combo");
        }
        
        // if clearing is enabled, remove all other sprites from combo
        if (clearOther) inv = inv.Select(slot => slot with { Slot = 0 }).ToList();
        
        // activate new combo
        for (var slot = 0; slot < combo.Count; slot++)
        {
            // clear existing sprite in slot
            var existingSprite = inv.FindIndex(invSlot => invSlot.Slot == slot + 1);
            if(existingSprite != -1) inv[existingSprite] = inv[existingSprite] with { Slot = 0 };
            
            // set target sprite in slot
            var targetSprite = inv.FindIndex(invSlot => invSlot.SpriteId == combo[slot]);
            if(targetSprite != -1) inv[targetSprite] = inv[targetSprite] with { Slot = slot + 1 };
            else throw new UserOperationException($"The user does not own the sprite {combo[slot]}");
        }
        
        // save combo
        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == login) ?? throw new EntityNotFoundException("The cant be activated because member doesn't exist");
        memberEntity.Sprites = InventoryHelper.SerializeSpriteInventory(inv);
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }
}