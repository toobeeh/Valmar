using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Implementation;

namespace tobeh.Valmar.Server.Grpc;

public class DropsGrpcService(
    ILogger<DropsGrpcService> logger, 
    IDropsDomainService dropsService) : Drops.DropsBase
{
    public override async Task<Empty> ScheduleDrop(ScheduleDropRequest request, ServerCallContext context)
    {
        logger.LogTrace("ScheduleDrop(request={request})", request);

        await dropsService.ScheduleDrop(request.DelaySeconds, request.EventDropId);
        return new Empty();
    }

    public override async Task<CurrentBoostFactorReply> GetCurrentBoostFactor(Empty request, ServerCallContext context)
    {
        logger.LogTrace("GetCurrentBoostFactor(empty)");
        
        var boost = await dropsService.GetCurrentDropBoost();
        return new CurrentBoostFactorReply { Boost = boost };
    }

    public override async Task<DropDelayBoundsReply> CalculateDropDelayBounds(CalculateDelayRequest request, ServerCallContext context)
    {
        logger.LogTrace("CalculateDropDelayBounds(request={request})", request);
        
        var bounds = await dropsService.CalculateDropDelayBounds(request.OnlinePlayerCount, request.BoostFactor);

        return new DropDelayBoundsReply { MinDelaySeconds = bounds.Item1, MaxDelaySeconds = bounds.Item2 };
    }
}