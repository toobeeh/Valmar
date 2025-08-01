using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes.Param;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class AdminGrpcService(
    ILogger<AdminGrpcService> logger,
    IMapper mapper,
    IAdminDomainService adminService) : Admin.AdminBase
{
    public override async Task<Empty> UpdateMemberFlags(UpdateMemberFlagsRequest request, ServerCallContext context)
    {
        logger.LogTrace("UpdateMemberFlags(request={request})", request);

        await adminService.SetPermissionFlag(request.MemberIds, request.FlagId, request.State, request.InvertOthers);
        return new Empty();
    }

    public override async Task<BubbleTracesCreatedMessage> CreateBubbleTraces(Empty request, ServerCallContext context)
    {
        logger.LogTrace("CreateBubbleTraces(empty)");
        var traces = await adminService.CreateBubbleTraces();
        return new BubbleTracesCreatedMessage { DailyPlayers = traces };
    }

    public override async Task<Empty> ClearVolatileData(Empty request, ServerCallContext context)
    {
        logger.LogTrace("ClearVolatileData(empty)");
        await adminService.ClearVolatileData();
        return new Empty();
    }

    public override async Task<Empty> IncrementMemberBubbles(IncrementMemberBubblesRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("IncrementMemberBubbles(request={request})", request);
        await adminService.IncrementMemberBubbles(request.MemberLogins);
        return new Empty();
    }

    public override async Task<Empty> SetOnlineItems(SetOnlineItemsRequest request, ServerCallContext context)
    {
        logger.LogTrace("SetOnlineItems(request={request})", request);
        await adminService.WriteOnlineItems(
            request.Items.Select(mapper.Map<OnlineItemMessage, OnlineItemDdo>).ToList());
        return new Empty();
    }

    public override async Task<Empty> ReevaluateDropChunks(Empty request, ServerCallContext context)
    {
        logger.LogTrace("ReevaluateDropChunks(empty)");
        await adminService.ReevaluateDropChunks();
        return new Empty();
    }

    public override async Task GetTemporaryPatrons(Empty request,
        IServerStreamWriter<TemporaryPatronMessage> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetTemporaryPatrons(empty)");

        var patrons = await adminService.GetTemporaryPatronLogins();
        await responseStream.WriteAllMappedAsync(patrons, patron => new() { Login = patron });
    }

    public override async Task GetOnlineItems(Empty request, IServerStreamWriter<OnlineItemMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetOnlineItems(empty)");

        var items = await adminService.GetAllOnlineItems();
        await responseStream.WriteAllMappedAsync(items, mapper.Map<OnlineItemDdo, OnlineItemMessage>);
    }
}