using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Database;

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
        
        CreateMap<LeagueSeasonMemberEvaluationDdo, LeagueSeasonMemberEvaluationReply>();
    }
}