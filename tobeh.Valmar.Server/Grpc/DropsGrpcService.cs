using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;

namespace tobeh.Valmar.Server.Grpc;

public class DropsGrpcService(
    ILogger<DropsGrpcService> logger,
    IDropsDomainService dropsService,
    IMapper mapper
) : Drops.DropsBase
{
    public override async Task<Empty> ScheduleDrop(ScheduleDropRequest request, ServerCallContext context)
    {
        logger.LogTrace("ScheduleDrop(request={request})", request);

        await dropsService.ScheduleDrop(request.DelaySeconds, request.EventDropId);
        return new Empty();
    }

    public override async Task<ScheduledDropMessage> GetScheduledDrop(Empty request, ServerCallContext context)
    {
        logger.LogTrace("GetScheduledDrop(empty)");

        var drop = await dropsService.GetScheduledDrop();
        return mapper.Map<ScheduledDropMessage>(drop);
    }

    public override async Task<CurrentBoostFactorReply> GetCurrentBoostFactor(Empty request, ServerCallContext context)
    {
        logger.LogTrace("GetCurrentBoostFactor(empty)");

        var boost = await dropsService.GetCurrentDropBoost();
        return new CurrentBoostFactorReply { Boost = boost };
    }

    public override async Task<DropDelayBoundsReply> CalculateDropDelayBounds(CalculateDelayRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("CalculateDropDelayBounds(request={request})", request);

        var bounds = await dropsService.CalculateDropDelayBounds(request.OnlinePlayerCount, request.BoostFactor);

        return new DropDelayBoundsReply { MinDelaySeconds = bounds.Item1, MaxDelaySeconds = bounds.Item2 };
    }

    public override async Task<ClaimDropResultMessage> ClaimDrop(ClaimDropMessage request, ServerCallContext context)
    {
        logger.LogTrace("ClaimDrop(request={request})", request);

        var result = await dropsService.ClaimDrop(request.DropId);
        return mapper.Map<ClaimDropResultMessage>(result);
    }

    public override async Task<Empty> LogDropClaim(LogDropMessage request, ServerCallContext context)
    {
        logger.LogTrace("LogDropClaim(request={request})", request);

        await dropsService.LogDropClaim(request.DropId, request.DropId, request.ClaimTimestamp, request.LobbyKey,
            request.CatchMs, request.EventDropId);
        return new Empty();
    }

    public override async Task<Empty> RewardDrop(RewardDropMessage request, ServerCallContext context)
    {
        logger.LogTrace("RewardDrop(request={request})", request);

        await dropsService.RewardDrop(request.Login, request.EventDropId, request.Value);
        return new Empty();
    }
}