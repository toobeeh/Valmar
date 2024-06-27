using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;
using tobeh.Valmar.Server.Util;
using tobeh.Valmar.Server.Util.NChunkTree.Bubbles;
using tobeh.Valmar.Server.Util.NChunkTree.Drops;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class InventoryDomainService(
    ILogger<InventoryDomainService> logger,
    PalantirContext db,
    ISpritesDomainService spritesService,
    IScenesDomainService scenesService,
    IEventsDomainService eventsService,
    IStatsDomainService statsService,
    DropChunkTreeProvider dropChunks,
    IAwardsDomainService awardsService,
    BubbleChunkTreeProvider bubbleChunks) : IInventoryDomainService
{
    public Task<List<MemberSpriteSlotDdo>> GetMemberSpriteInventory(MemberDdo member)
    {
        logger.LogTrace("GetMemberSpriteInventory(member={member})", member);

        var inv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites);
        return Task.FromResult(inv);
    }

    public Task<SceneInventoryDdo> GetMemberSceneInventory(MemberDdo member)
    {
        logger.LogTrace("GetMemberSceneInventory(member={member})", member);

        var inv = InventoryHelper.ParseSceneInventory(member.Scenes);
        return Task.FromResult(inv);
    }

    public async Task<BubbleCreditDdo> GetBubbleCredit(MemberDdo member)
    {
        logger.LogTrace("GetBubbleCredit(member={member})", member);

        var spriteInv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites)
            .Select(inv => inv.SpriteId).ToArray();
        var invSum = await db.Sprites.Where(sprite => sprite.EventDropId == 0 && spriteInv.Contains(sprite.Id))
            .SumAsync(sprite => sprite.Cost);

        var sceneInv = InventoryHelper.ParseSceneInventory(member.Scenes).Scenes.Select(scene => scene.SceneId)
            .ToArray();
        var regularScenes = await db.Scenes
            .Where(scene => scene.EventId == 0 && !scene.Exclusive).ToListAsync();
        var regularInvScenes = sceneInv.Where(scene => regularScenes.Any(s => s.Id == scene)).ToList();
        var sceneSum = regularInvScenes.Select((scene, index) => SceneHelper.GetScenePrice(index)).Sum();

        var totalBubbles = member.Bubbles + (int)Math.Floor(member.Drops * 50);
        var availableBubbles = totalBubbles - invSum - sceneSum;

        return new BubbleCreditDdo(totalBubbles, availableBubbles, member.Bubbles);
    }

    public async Task<DropCreditDdo> GetDropCredit(MemberDdo member)
    {
        logger.LogTrace("GetDropCredit(member={member})", member);

        var chunk = dropChunks.GetTree().Chunk;
        var legacyCount =
            (await db.LegacyDropCounts.FirstOrDefaultAsync(entity => entity.Login == member.Login))?.Count ?? 0;
        var leagueCount = await chunk.GetLeagueCount(member.DiscordId.ToString());

        return new DropCreditDdo(member.Drops, legacyCount + leagueCount);
    }

    public async Task<List<EventCreditDdo>> GetEventCredit(MemberDdo member, List<int> eventDropIds)
    {
        logger.LogTrace("GetEventCredit(member={member})", member);

        var inv = (await GetMemberSpriteInventory(member)).Select(inv => inv.SpriteId).ToArray();

        var spentCredits = await db.Sprites
            .Where(sprite => eventDropIds.Contains(sprite.EventDropId) && inv.Contains(sprite.Id))
            .GroupBy(sprite => sprite.EventDropId)
            .Select(group => new { EventDropId = group.Key, Cost = group.Sum(sprite => sprite.Cost) })
            .ToListAsync();

        var credits = await db.EventCredits
            .Where(credit => credit.Login == member.Login && eventDropIds.Contains(credit.EventDropId))
            .ToListAsync();

        var creditResults = eventDropIds.Select(dropId =>
        {
            var totalCredit =
                (int)Math.Floor(credits.FirstOrDefault(credit => credit.EventDropId == dropId)?.Credit ?? 0);
            var availableCredit = totalCredit -
                                  (spentCredits.FirstOrDefault(spent => spent.EventDropId == dropId)?.Cost ?? 0);
            return new EventCreditDdo(dropId, totalCredit, availableCredit);
        }).ToList();

        return creditResults;
    }

    public async Task BuySprite(MemberDdo member, int spriteId)
    {
        logger.LogTrace("BuySprite(member={member}, spriteId={spriteId})", member, spriteId);

        // check if the sprite is already in the inventory
        var inv = await GetMemberSpriteInventory(member);
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
            var credit = (await GetEventCredit(member, [sprite.EventDropId]))
                .FirstOrDefault(credit => credit.EventDropId == sprite.EventDropId)?.AvailableCredit ?? 0;
            if (credit < sprite.Cost)
                throw new UserOperationException(
                    $"The sprite price ({sprite.Cost}) exceeds the available event credit ({credit}) of the event drop {sprite.Name} ({sprite.EventDropId})");
        }
        else
        {
            var credit = await GetBubbleCredit(member);
            if (credit.AvailableCredit < sprite.Cost)
                throw new UserOperationException(
                    $"The sprite price ({sprite.Cost}) exceeds the available bubble credit ({credit.AvailableCredit})");
        }

        var memberEntity = await db.Members.FirstOrDefaultAsync(entity => entity.Login == member.Login) ??
                           throw new EntityNotFoundException(
                               "The sprite can't be added to member inventory, because member doesn't exist");
        inv.Add(new MemberSpriteSlotDdo(0, sprite.Id, null));
        memberEntity.Sprites = InventoryHelper.SerializeSpriteInventory(inv);
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }

    public Task<int> GetSpriteSlotCount(MemberDdo member)
    {
        logger.LogTrace("GetSpriteSlotCount(member={member})", member);

        var isPatron = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Patron);
        var isAdmin = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Admin);

        var slots = (isPatron ? 1 : 0) + (isAdmin ? 100 : 0) + InventoryHelper.GetSlotBaseCount(member.Drops);
        return Task.FromResult(slots);
    }

    public async Task SetColorShiftConfiguration(MemberDdo member, Dictionary<int, int?> colorShiftMap,
        bool clearOther = false)
    {
        logger.LogTrace(
            "SetColorShiftConfiguration(member={member}, colorShiftMap={colorShiftMap}, clearOther={clearOther})",
            member, colorShiftMap, clearOther);

        var inv = await GetMemberSpriteInventory(member);
        var isPatron = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Patron);
        var isAdmin = FlagHelper.HasFlag(member.Flags, MemberFlagDdo.Admin);

        // if clearing, remove all configs
        if (clearOther)
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

        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == member.Login) ??
                           throw new EntityNotFoundException(
                               "The color shift configuration can't be saved because member doesn't exist");
        memberEntity.RainbowSprites = configString;
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }

    public async Task UseScene(MemberDdo member, int? sceneId, int? sceneShift)
    {
        logger.LogTrace("UseScene(member={member}, sceneId={sceneId}, sceneShift={sceneShift})", member, sceneId,
            sceneShift);

        var inv = InventoryHelper.ParseSceneInventory(member.Scenes);

        // check if user owns the scene 
        if (sceneId is { } sceneIdValue &&
            !inv.Scenes.Any(scene => scene.SceneId == sceneId && scene.SceneShift == sceneShift))
        {
            throw new UserOperationException($"The user does not own the scene {sceneIdValue} with shift {sceneShift}");
        }

        // set new active scene
        inv = inv with { ActiveId = sceneId, ActiveShift = sceneShift };

        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == member.Login) ??
                           throw new EntityNotFoundException(
                               "The scene can't be activated because member doesn't exist");
        memberEntity.Scenes = InventoryHelper.SerializeSceneInventory(inv);
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }

    public async Task BuyScene(MemberDdo member, int sceneId, int? sceneShift)
    {
        logger.LogTrace("BuyScene(member={member}, sceneId={sceneId}, sceneShift={sceneShift})", member, sceneId,
            sceneShift);

        var inv = InventoryHelper.ParseSceneInventory(member.Scenes);

        // check if user owns the scene 
        if (inv.Scenes.Any(scene => scene.SceneId == sceneId && scene.SceneShift == sceneShift))
        {
            throw new UserOperationException($"The user already owns the scene {sceneId}");
        }

        // check if the scene is available for purchase
        var scene = await scenesService.GetSceneById(sceneId);
        if (scene.Exclusive)
        {
            throw new UserOperationException(
                $"The scene {scene.Name} ({scene.Id}) is exclusive and can't be purchased");
        }

        // check if the scene has theme
        if (sceneShift is { } shift)
        {
            var themes = await scenesService.GetThemesOfScene(sceneId);
            if (themes.All(theme => theme.Shift != shift))
            {
                throw new UserOperationException(
                    $"The scene {scene.Name} ({scene.Id}) has no theme with shift {shift}");
            }
        }

        // if theme, check if user owns the base scene 
        if (sceneShift is not null &&
            !inv.Scenes.Any(invScene => invScene.SceneId == sceneId && invScene.SceneShift is null))
        {
            throw new UserOperationException(
                $"The user does not own the scene {sceneId} which is required for this theme");
        }

        // check if scene is event scene or regular scene and verify price
        if (scene.EventId != 0)
        {
            var sceneEvent = await eventsService.GetEventById(scene.EventId);
            var eventPrice = EventHelper.GetEventScenePrice(sceneEvent.Length);
            var traceBeforeStart =
                sceneEvent.StartDate
                    .AddDays(-1); // traces are record from end of day - event start credit is from day before start
            var eventBubblesCollected =
                await statsService.GetMemberBubblesInRange(member.Login, traceBeforeStart, sceneEvent.EndDate);
            var totalCollectedDuringEvent = eventBubblesCollected.EndAmount - eventBubblesCollected.StartAmount;

            if (eventPrice > totalCollectedDuringEvent)
            {
                throw new UserOperationException(
                    $"The scene price ({eventPrice}) exceeds the available bubbles ({totalCollectedDuringEvent})");
            }
        }
        else
        {
            var bubbleCredit = await GetBubbleCredit(member);

            var sceneInv = inv.Scenes.Select(s => s.SceneId).ToArray();
            var nonEventScenes = await db.Scenes
                .Where(s => s.EventId == 0 && !s.Exclusive && sceneInv.Contains(s.Id)).ToListAsync();
            var scenePrice = SceneHelper.GetScenePrice(nonEventScenes.Count);

            if (scenePrice > bubbleCredit.AvailableCredit)
            {
                throw new UserOperationException(
                    $"The scene price ({scenePrice}) exceeds the available bubble credit ({bubbleCredit.AvailableCredit})");
            }
        }

        // add scene to inventory
        inv.Scenes.Add(new SceneInventoryItemDdo(sceneId, sceneShift));
        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == member.Login) ??
                           throw new EntityNotFoundException(
                               "The scene can't be activated because member doesn't exist");
        memberEntity.Scenes = InventoryHelper.SerializeSceneInventory(inv);
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }

    public async Task UseSpriteCombo(MemberDdo member, List<int?> combo, bool clearOther = false)
    {
        logger.LogTrace("UseSpriteCombo(member={member}, combo={combo}, clearOther={clearOther})", member, combo,
            clearOther);

        var inv = InventoryHelper.ParseSpriteInventory(member.Sprites, member.RainbowSprites);

        // check if all sprites are in the inventory
        if (combo.Any(id => id is not null && inv.All(slot => slot.SpriteId != id)))
        {
            throw new UserOperationException($"The user does not own all sprites from the combo {combo}");
        }

        // check if all sprites are unique
        if (combo.Where(spt => spt is not null).Distinct().Count() != combo.Count(spt => spt is not null))
        {
            throw new UserOperationException("The combo contains duplicate sprites");
        }

        // check if user has enough sprite slots
        if (combo.Count > await GetSpriteSlotCount(member))
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
            if (existingSprite != -1) inv[existingSprite] = inv[existingSprite] with { Slot = 0 };

            // set target sprite in slot
            if (combo[slot] is not null)
            {
                var targetSprite = inv.FindIndex(invSlot => invSlot.SpriteId == combo[slot]);
                if (targetSprite != -1) inv[targetSprite] = inv[targetSprite] with { Slot = slot + 1 };
                else throw new UserOperationException($"The user does not own the sprite {combo[slot]}");
            }
        }

        // save combo
        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == member.Login) ??
                           throw new EntityNotFoundException("The cant be activated because member doesn't exist");
        memberEntity.Sprites = InventoryHelper.SerializeSpriteInventory(inv);
        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }

    public async Task<AwardInventoryDdo> GetMemberAwardInventory(int login)
    {
        logger.LogTrace("GetMemberAwardInventory(login={login})", login);

        var ownAvailableAwards = await db.Awardees
            .Where(awardee => awardee.OwnerLogin == login && awardee.AwardeeLogin == null).ToListAsync();
        var ownConsumedAwards = await db.Awardees
            .Where(awardee => awardee.OwnerLogin == login && awardee.AwardeeLogin != null).ToListAsync();
        var receivedAwards = await db.Awardees.Where(awardee => awardee.AwardeeLogin == login).ToListAsync();

        return new AwardInventoryDdo(ownAvailableAwards, ownConsumedAwards, receivedAwards);
    }

    public async Task<List<GalleryItemDdo>> GetImagesFromCloud(MemberDdo member, List<long> ids)
    {
        logger.LogTrace("GetImagesFromCloud(member={member}, ids={ids})", member, ids);

        var images = await db.CloudTags.Where(tag => tag.Owner == member.Login && ids.Contains(tag.ImageId))
            .ToListAsync();

        return images.Select(image => new GalleryItemDdo(
            image.ImageId,
            $"https://cloud.typo.rip/{member.DiscordId}/{image.ImageId}/image.png",
            image.Title,
            image.Author,
            DateTimeOffset.FromUnixTimeMilliseconds(image.Date),
            image.Language,
            image.Own,
            image.Private
        )).ToList();
    }

    public async Task<List<AwardEntity>> OpenAwardPack(MemberDdo member, int rarityLevel)
    {
        logger.LogTrace("OpenAwardPack(member={member}, rarityLevel={rarityLevel})", member, rarityLevel);

        if (member.NextAwardPackDate > DateTimeOffset.UtcNow)
        {
            throw new UserOperationException("The award pack can't be opened because the cooldown hasn't expired yet");
        }

        var awards = await awardsService.GetAllAwards();
        double[] rarityRanges = rarityLevel switch
        {
            1 => [0.55, 0.8, 0.97],
            2 => [0.4, 0.7, 0.95],
            3 => [0.3, 0.5, 0.91],
            _ => [0.2, 0.4, 0.85]
        };

        AwardEntity GetRandomAward()
        {
            var random = new Random().NextDouble();
            var rarity = random < rarityRanges[0] ? 1 : random < rarityRanges[1] ? 2 : random < rarityRanges[2] ? 3 : 4;
            var availableAwards = awards.Where(award => award.Rarity == rarity).ToList();
            var randomIndex = new Random().NextInt64(0, availableAwards.Count - 1);
            var randomAward = availableAwards[(int)randomIndex];
            awards.Remove(randomAward);

            return randomAward;
        }

        var result = Enumerable.Repeat(GetRandomAward, rarityLevel > 1 ? 2 : 1).Select(func => func.Invoke()).ToList();

        var memberEntity = await db.Members.FirstOrDefaultAsync(memberEntity => memberEntity.Login == member.Login) ??
                           throw new EntityNotFoundException(
                               "The award pack can't be opened because member doesn't exist");
        memberEntity.AwardPackOpened = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        db.Update(memberEntity);

        var awardees = result.Select(award => new AwardeeEntity { Award = (short)award.Id, OwnerLogin = member.Login })
            .ToList();
        db.Awardees.AddRange(awardees);

        await db.SaveChangesAsync();
        return result;
    }

    public async Task<DateTimeOffset> GetFirstSeenDate(int login) // TODO move to stats
    {
        logger.LogTrace("GetFirstSeenDate(login={login})", login);

        return await bubbleChunks.GetTree().Chunk.GetFirstSeenDate(login) ?? DateTimeOffset.Now;
    }

    public async Task<GiftLossRateDdo> GetGiftLossRateBase(MemberDdo member, List<SpriteDdo> eventSprites)
    {
        logger.LogTrace("GetGiftLossRateBase(member={member}, eventSprites={eventSprites})", member, eventSprites);

        var totalValue = eventSprites.Sum(sprite => sprite.Cost);
        var eventDetails = await dropChunks.GetTree().Chunk
            .GetEventLeagueDetails(eventSprites.Select(sprite => sprite.EventDropId).ToArray(),
                member.DiscordId.ToString());
        var lossRate = EventHelper.CalculateCurrentGiftLossRate(totalValue, eventDetails.TotalCollected);

        return new GiftLossRateDdo(totalValue, eventDetails.TotalCollected, lossRate);
    }

    public async Task<EventResultDdo> GetEventProgress(MemberDdo member)
    {
        logger.LogTrace("GetTotalEventDropsCollected(member={member})", member);
        var eventDetails = await dropChunks.GetTree().Chunk
            .GetEventLeagueDetails(null, member.DiscordId.ToString());

        return eventDetails;
    }

    public async Task<EventCreditGiftResultDdo> GiftEventCredit(MemberDdo fromMember, MemberDdo toMember, int amount,
        EventDropDdo eventDrop, GiftLossRateDdo lossRate)
    {
        logger.LogTrace(
            "GiftEventCredit(fromMember={fromMember}, toMember={toMember}, amount={amount}, eventDropId={eventDropId}, lossRate={lossRate})",
            fromMember, toMember, amount, eventDrop, lossRate);

        var eventDrops = (await eventsService.GetEventDrops(eventDrop.EventId)).Select(drop => drop.Id).ToArray();
        var eventCredit = await GetEventCredit(fromMember, eventDrops.ToList());

        var availableCredit =
            eventCredit.FirstOrDefault(credit => credit.EventDropId == eventDrop.Id)?.AvailableCredit ?? 0;
        if (amount > availableCredit)
        {
            throw new UserOperationException("The amount of gifted credit exceeds the available credit");
        }

        var loss = (int)Math.Ceiling(EventHelper.CalculateRandomGiftLoss(lossRate.LossRateBase, amount));

        var fromEntity = await db.EventCredits.FirstAsync(credit =>
            credit.Login == fromMember.Login && credit.EventDropId == eventDrop.Id);
        var toEntity = await db.EventCredits.FirstOrDefaultAsync(credit =>
            credit.Login == toMember.Login && credit.EventDropId == eventDrop.Id);
        if (toEntity == null)
        {
            toEntity = new EventCreditEntity
            {
                Login = toMember.Login,
                EventDropId = eventDrop.Id,
                Credit = 0
            };
            db.EventCredits.Add(toEntity);
            await db.SaveChangesAsync();
        }

        fromEntity.Credit -= amount;
        toEntity.Credit += amount - loss;

        db.EventCredits.UpdateRange([fromEntity, toEntity]);
        await db.SaveChangesAsync();

        return new EventCreditGiftResultDdo(lossRate, loss, amount);
    }

    public async Task SetPatronEmoji(MemberDdo member, string? emoji)
    {
        logger.LogTrace("SetPatronEmoji(member={member}, emoji={emoji})", member, emoji);

        if (!member.MappedFlags.Contains(MemberFlagDdo.Patron))
        {
            throw new UserOperationException("The user is not a patron and is not allowed to set a patron emoji.");
        }

        var memberEntity = await db.Members.FirstAsync(entity => entity.Login == member.Login);
        memberEntity.Emoji = emoji;

        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }

    public async Task SetPatronizedMember(MemberDdo member, long? patronizedMemberDiscordId)
    {
        logger.LogTrace("SetPatronizedMember(member={member}, patronizedMemberDiscordId={patronizedMemberDiscordId})",
            member, patronizedMemberDiscordId);

        if (!member.MappedFlags.Any(f => f is MemberFlagDdo.Patronizer or MemberFlagDdo.Admin))
        {
            throw new UserOperationException(
                "The user is not a patronizer and is not allowed to set a patronized member.");
        }

        if (member.NextPatronizeDate > DateTimeOffset.UtcNow)
        {
            throw new UserOperationException(
                $"Patronized member cooldown is active until {member.NextPatronizeDate:d}");
        }

        var memberEntity = await db.Members.FirstAsync(entity => entity.Login == member.Login);
        memberEntity.Patronize =
            InventoryHelper.SerializePatronizedMember(
                new Tuple<long?, DateTimeOffset>(patronizedMemberDiscordId, DateTimeOffset.UtcNow));

        db.Members.Update(memberEntity);
        await db.SaveChangesAsync();
    }
}