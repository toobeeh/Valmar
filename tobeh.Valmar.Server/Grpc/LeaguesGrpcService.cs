using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;

namespace tobeh.Valmar.Server.Grpc;

public class LeaguesGrpcService(
    ILogger<LeaguesGrpcService> logger,
    IMapper mapper,
    ILeaguesDomainService leaguesService) : Leagues.LeaguesBase
{
    public override async Task<LeagueSeasonEvaluationReply> EvaluateCurrentLeagueSeason(Empty request,
        ServerCallContext context)
    {
        logger.LogTrace("GetCurrentLobbies(empty)");

        var year = DateTimeOffset.UtcNow.Year;
        var month = DateTimeOffset.UtcNow.Month;

        var evaluation = await leaguesService.EvaluateLeagueSeason(year, month);

        return mapper.Map<LeagueSeasonEvaluationReply>(evaluation);
    }

    public override async Task<LeagueSeasonEvaluationReply> EvaluateLeagueSeason(EvaluateSeasonRequest request,
        ServerCallContext context)
    {
        logger.LogTrace("EvaluateLeagueSeason({request})", request);

        var evaluation = await leaguesService.EvaluateLeagueSeason(request.Year, request.Month);

        return mapper.Map<LeagueSeasonEvaluationReply>(evaluation);
    }

    public override async Task<LeagueSeasonMemberEvaluationReply> EvaluateMemberCurrentLeagueSeason(
        EvaluateMemberCurrentSeasonRequest request, ServerCallContext context)
    {
        logger.LogTrace("EvaluateMemberCurrentLeagueSeason({request})", request);

        var year = DateTimeOffset.UtcNow.Year;
        var month = DateTimeOffset.UtcNow.Month;

        var evaluation = await leaguesService.EvaluateOwnLeagueSeason(year, month, request.Login);

        return mapper.Map<LeagueSeasonMemberEvaluationReply>(evaluation);
    }

    public override async Task<LeagueSeasonSplitEvaluationReply> EvaluateLeagueSeasonSplits(
        EvaluateSeasonRequest request, ServerCallContext context)
    {
        logger.LogTrace("EvaluateLeagueSeasonSplits({request})", request);

        var evaluation = await leaguesService.EvaluateLeagueSeasonSplits(request.Year, request.Month);

        return mapper.Map<LeagueSeasonSplitEvaluationReply>(evaluation);
    }

    public override async Task<LeagueSeasonMemberEvaluationReply> EvaluateMemberLeagueSeason(
        EvaluateMemberSeasonRequest request, ServerCallContext context)
    {
        logger.LogTrace("EvaluateMemberLeagueSeason({request})", request);

        var evaluation = await leaguesService.EvaluateOwnLeagueSeason(request.Year, request.Month, request.Login);

        return mapper.Map<LeagueSeasonMemberEvaluationReply>(evaluation);
    }
}