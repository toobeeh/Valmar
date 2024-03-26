using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Domain.Classes.Param;
using Valmar.Domain.Implementation;

namespace Valmar.Grpc;

public class StatsGrpcService(
    ILogger<StatsGrpcService> logger, 
    IMapper mapper,
    IStatsDomainService statsService) : Stats.StatsBase
{
    public override async Task<BubbleTimespanRangeReply> GetBubbleTimespanRange(BubbleTimespanRangeRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetBubbleTimespanRange(request={request})", request);

        var range = await statsService.GetMemberBubblesInRange(request.Login, request.StartDate.ToDateTimeOffset(),
            request.EndDate.ToDateTimeOffset());
        return mapper.Map<BubbleTimespanRangeReply>(range);
    }
}