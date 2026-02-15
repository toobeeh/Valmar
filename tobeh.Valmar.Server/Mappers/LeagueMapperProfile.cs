using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class LeagueMapperProfile : Profile
{
    public LeagueMapperProfile()
    {
        CreateMap<LeagueSeasonEvaluationDdo, LeagueSeasonEvaluationReply>();
        CreateMap<LeagueScoreRankingDdo, LeagueScoreRankingReply>();
        CreateMap<LeagueCountRankingDdo, LeagueCountRankingReply>();
        CreateMap<LeagueAverageWeightRankingDdo, LeagueAverageWeightRankingReply>();
        CreateMap<LeagueAverageTimeRankingDdo, LeagueAverageTimeRankingReply>();
        CreateMap<LeagueStreakRankingDdo, LeagueStreakRankingReply>();
        CreateMap<LeagueSeasonSplitEvaluationDdo, LeagueSeasonSplitEvaluationReply>();
        CreateMap<LeagueSeasonMemberSplitEvaluationDdo, LeagueSeasonMemberSplitEvaluationDdo>();
        CreateMap<LeagueSeasonMemberEvaluationDdo, LeagueSeasonMemberEvaluationReply>();
    }
}