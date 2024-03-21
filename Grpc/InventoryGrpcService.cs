using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Grpc.Utils;

namespace Valmar.Grpc;

public class InventoryGrpcService(
    ILogger<InventoryGrpcService> logger, 
    IMapper mapper,
    IEventsDomainService eventsService,
    IInventoryDomainService inventoryService) : Inventory.InventoryBase
{
    public override async Task<BubbleCreditReply> GetBubbleCredit(GetBubbleCreditRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetBubbleCredit(request={request})", request);

        var dropCredit = await inventoryService.GetDropCredit(request.Login);
        var credit = await inventoryService.GetBubbleCredit(request.Login, dropCredit);
        return mapper.Map<BubbleCreditReply>(credit);
    }

    public override async Task<DropCreditReply> GetDropCredit(GetDropCreditRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetDropCredit(request={request})", request);

        var dropCredit = await inventoryService.GetDropCredit(request.Login);
        return mapper.Map<DropCreditReply>(dropCredit);
    }

    public override async Task<SpriteSlotCountReply> GetSpriteSlotCount(GetSpriteSlotCountRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetSpriteSlotCount(request={request})", request);

        var count = await inventoryService.GetSpriteSlotCount(request.Login);
        return new SpriteSlotCountReply { UnlockedSlots = count };
    }

    public override async Task GetEventCredit(GetEventCreditRequest request, IServerStreamWriter<EventCreditReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetEventCreditRequest(request={request})", request);

        var drops = await eventsService.GetEventDropsOfEvent(request.EventId);
        var credit = await inventoryService.GetEventCredit(request.Login, drops.Select(drop => drop.EventDropId).ToList());
        await responseStream.WriteAllMappedAsync(credit, mapper.Map<EventCreditReply>);
    }

    public override async Task GetSpriteInventory(GetSpriteInventoryRequest request, IServerStreamWriter<SpriteSlotConfigurationReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetSpriteInventory(request={request})", request);

        var inv = await inventoryService.GetMemberSpriteInventory(request.Login);
        await responseStream.WriteAllMappedAsync(inv, mapper.Map<SpriteSlotConfigurationReply>);
    }

    public override async Task<Empty> BuySprite(BuySpriteRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetSpriteInventory(request={request})", request);
        
        await inventoryService.BuySprite(request.Login, request.SpriteId);
        return new Empty();
    }

    public override async Task<Empty> UseSpriteCombo(UseSpriteComboRequest request, ServerCallContext context)
    {
        logger.LogTrace("UseSpriteCombo(request={request})", request);
        
        var slots = Enumerable.Repeat(0, request.Combo.Max(slot => slot.SlotId)).ToList();
        foreach (var slot in request.Combo)
        {
            slots[slot.SlotId - 1] = slot.SpriteId;
        }
        
        await inventoryService.UseSpriteCombo(request.Login, slots);
        return new Empty();
    }

    public override async Task<Empty> SetSpriteColor(SetSpriteColorRequest request, ServerCallContext context)
    {
        return await base.SetSpriteColor(request, context);
    }
}