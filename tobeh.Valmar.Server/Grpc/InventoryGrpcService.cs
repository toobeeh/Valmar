using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Grpc.Utils;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Grpc;

public class InventoryGrpcService(
    ILogger<InventoryGrpcService> logger,
    IMapper mapper,
    IMembersDomainService membersService,
    IEventsDomainService eventsService,
    IStatsDomainService statsService,
    ISpritesDomainService spritesService,
    IInventoryDomainService inventoryService) : Inventory.InventoryBase
{
    public override async Task<BubbleCreditReply> GetBubbleCredit(GetBubbleCreditRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("GetBubbleCredit(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var credit = await inventoryService.GetBubbleCredit(member);
        return mapper.Map<BubbleCreditReply>(credit);
    }

    public override async Task<DropCreditReply> GetDropCredit(GetDropCreditRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetDropCredit(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var dropCredit = await inventoryService.GetDropCredit(member);
        return mapper.Map<DropCreditReply>(dropCredit);
    }

    public override async Task<SpriteSlotCountReply> GetSpriteSlotCount(GetSpriteSlotCountRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("GetSpriteSlotCount(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var count = await inventoryService.GetSpriteSlotCount(member);
        var remainingDrops = await inventoryService.GetNextSlotRemainingDrops(member);
        return new SpriteSlotCountReply { UnlockedSlots = count, DropsUntilNextSlot = remainingDrops };
    }

    public override async Task GetEventCredit(GetEventCreditRequest request,
        IServerStreamWriter<EventCreditReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetEventCreditRequest(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var drops = await eventsService.GetEventDrops(request.EventId);
        var credit = await inventoryService.GetEventCredit(member, drops.Select(drop => drop.Id).ToList());
        await responseStream.WriteAllMappedAsync(credit, mapper.Map<EventCreditReply>);
    }

    public override async Task<EventProgressMessage> GetEventProgress(GetEventProgressRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("GetEventProgress(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var progress = await inventoryService.GetEventProgress(member);

        return mapper.Map<EventProgressMessage>(progress);
    }

    public override async Task GetSpriteInventory(GetSpriteInventoryRequest request,
        IServerStreamWriter<SpriteSlotConfigurationReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetSpriteInventory(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var inv = await inventoryService.GetMemberSpriteInventory(member);
        await responseStream.WriteAllMappedAsync(inv, mapper.Map<SpriteSlotConfigurationReply>);
    }

    public override async Task<Empty> BuySprite(BuySpriteRequest request, ServerCallContext context)
    {
        logger.LogTrace("BuySprite(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        await inventoryService.BuySprite(member, request.SpriteId);
        return new Empty();
    }

    public override async Task<Empty> UseSpriteCombo(UseSpriteComboRequest request, ServerCallContext context)
    {
        logger.LogTrace("UseSpriteCombo(request={request})", request);

        var slots = Enumerable.Repeat<int?>(null, request.Combo.Count == 0 ? 0 : request.Combo.Max(slot => slot.SlotId))
            .ToList();
        foreach (var slot in request.Combo)
        {
            slots[slot.SlotId - 1] = slot.SpriteId;
        }

        var member = await membersService.GetMemberByLogin(request.Login);
        await inventoryService.UseSpriteCombo(member, slots, request.ClearOtherSlots);
        return new Empty();
    }

    public override async Task<Empty> SetSpriteColorConfiguration(SetSpriteColorRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("SetSpriteColorConfiguration(request={request})", request);

        var configParams = request.ColorConfig.ToDictionary(config => config.SpriteId, config => config.ColorShift);
        var member = await membersService.GetMemberByLogin(request.Login);
        await inventoryService.SetColorShiftConfiguration(member, configParams, request.ClearOtherConfigs);
        return new Empty();
    }

    public override async Task<SceneInventoryReply> GetSceneInventory(GetSceneInventoryRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("GetSceneInventory(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var inv = await inventoryService.GetMemberSceneInventory(member);
        return mapper.Map<SceneInventoryReply>(inv);
    }

    public override async Task<Empty> BuyScene(BuySceneRequest request, ServerCallContext context)
    {
        logger.LogTrace("BuyScene(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        await inventoryService.BuyScene(member, request.SceneId, request.SceneShift);
        return new Empty();
    }

    public override async Task<Empty> UseScene(UseSceneRequest request, ServerCallContext context)
    {
        logger.LogTrace("UseScene(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        await inventoryService.UseScene(member, request.SceneId, request.SceneShift);
        return new Empty();
    }

    public override Task<ScenePriceReply> GetScenePrice(ScenePriceRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetScenePrice(request={request})", request);

        var nextPrice = SceneHelper.GetScenePrice(request.BoughtSceneCount);
        var spentAmount = request.BoughtSceneCount == 0
            ? 0
            : Enumerable.Range(0, request.BoughtSceneCount).Sum(SceneHelper.GetScenePrice);
        return Task.FromResult(new ScenePriceReply { NextPrice = nextPrice, TotalBubblesSpent = spentAmount });
    }

    public override async Task<AwardInventoryMessage> GetAwardInventory(GetAwardInventoryMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("GetAwardInventory(request={request})", request);

        var inv = await inventoryService.GetMemberAwardInventory(request.Login);
        return mapper.Map<AwardInventoryMessage>(inv);
    }

    public override async Task<AwardPackLevelMessage> GetAwardPackLevel(GetAwardPackLevelMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("GetAwardPackLevel(request={request})", request);

        var bubblesCollected = await statsService.GetMemberBubblesInRange(request.Login,
            DateTimeOffset.UtcNow.AddDays(-8), DateTimeOffset.UtcNow);
        var diff = (bubblesCollected.EndAmount - bubblesCollected.StartAmount) ?? 0;
        var level = InventoryHelper.GetAwardPackRarity(diff);

        return new AwardPackLevelMessage
        {
            CollectedBubbles = diff,
            Level = mapper.Map<AwardRarityMessage>(level)
        };
    }

    public override async Task GetGalleryItems(GetGalleryItemsMessage request,
        IServerStreamWriter<GalleryItemMessage> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetGalleryItems(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var images = await inventoryService.GetImagesFromCloud(member, request.ImageIds.ToList());
        await responseStream.WriteAllMappedAsync(images, mapper.Map<GalleryItemMessage>);
    }

    public override async Task<AwardPackResultMessage> OpenAwardPack(OpenAwardPackMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("OpenAwardPack(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        var bubblesCollected = await statsService.GetMemberBubblesInRange(request.Login,
            DateTimeOffset.UtcNow.AddDays(-8), DateTimeOffset.UtcNow);
        var diff = (bubblesCollected.EndAmount - bubblesCollected.StartAmount) ?? 0;
        var level = InventoryHelper.GetAwardPackRarity(diff);

        var awards = await inventoryService.OpenAwardPack(member, level);

        return new AwardPackResultMessage
        {
            Awards = { mapper.Map<List<AwardReply>>(awards) }
        };
    }

    public override async Task<FirstSeenMessage> GetFirstSeenDate(GetFirstSeenDateRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("GetFirstSeenDate(request={request})", request);

        var firstSeen = await inventoryService.GetFirstSeenDate(request.Login);
        return new FirstSeenMessage { FirstSeen = mapper.Map<Timestamp>(firstSeen) };
    }

    public override async Task<GiftLossMessage> GiftEventCredit(GiftEventCreditMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("GiftEventCredit(request={request})", request);

        var evtDrop = await eventsService.GetEventDropById(request.EventDropId);
        var eventSprites = await spritesService.GetAllSprites(evtDrop.EventId);
        var member = await membersService.GetMemberByLogin(request.SenderLogin);
        var recipient = await membersService.GetMemberByLogin(request.RecipientLogin);
        var lossRate = await inventoryService.GetGiftLossRateBase(member, eventSprites);
        var result = await inventoryService.GiftEventCredit(member, recipient, request.Amount, evtDrop, lossRate);

        return mapper.Map<GiftLossMessage>(result);
    }

    public override async Task<GiftLossRateMessage> GetGiftLossRate(GetGiftLossRateMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("GetGiftLossRate(request={request})", request);

        var eventSprites = await spritesService.GetAllSprites(request.EventId);
        var member = await membersService.GetMemberByLogin(request.Login);
        var lossRate = await inventoryService.GetGiftLossRateBase(member, eventSprites);

        return mapper.Map<GiftLossRateMessage>(lossRate);
    }

    public override async Task<Empty> PatronizeMember(PatronizeMemberMessage request, ServerCallContext context)
    {
        logger.LogTrace("PatronizeMember(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        await inventoryService.SetPatronizedMember(member, request.PatronizedDiscordId);

        return new Empty();
    }

    public override async Task<Empty> SetPatronEmoji(SetPatronEmojiMessage request, ServerCallContext context)
    {
        logger.LogTrace("SetPatronEmoji(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        await inventoryService.SetPatronEmoji(member, request.Emoji);

        return new Empty();
    }
}