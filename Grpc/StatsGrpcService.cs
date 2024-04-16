using AutoMapper;
using Grpc.Core;
using Valmar.Domain;

namespace Valmar.Grpc;

public class StatsGrpcService(
    ILogger<StatsGrpcService> logger, 
    IMapper mapper,
    IMembersDomainService membersService,
    IStatsDomainService statsService) : Stats.StatsBase
{
    public override async Task<BubbleTimespanRangeReply> GetBubbleTimespanRange(BubbleTimespanRangeRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetBubbleTimespanRange(request={request})", request);

        var range = await statsService.GetMemberBubblesInRange(request.Login, request.StartDate.ToDateTimeOffset(),
            request.EndDate.ToDateTimeOffset());
        return mapper.Map<BubbleTimespanRangeReply>(range);
    }

    public override async Task<LeaderboardMessage> GetLeaderboard(GetLeaderboardMessage request, ServerCallContext context)
    {
        logger.LogTrace("GetLeaderboard(request={request})", request);

        var members = request.GuildToken is { } token
            ? await membersService.GetGuildMembers(token)
            : await membersService.GetAllMembers();
        
        var leaderboard = await statsService.CreateLeaderboard(members, request.Mode);
        
        return new LeaderboardMessage { Entries = { mapper.Map<List<LeaderboardRankMessage>>(leaderboard) } };
    }
}