using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

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