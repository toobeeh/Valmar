using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class LeagueMapperProfile : Profile
{
    public LeagueMapperProfile()
    {
        CreateMap<LeagueSeasonEvaluationDdo, LeagueSeasonEvaluationReply>();
        CreateMap<LeagueSeasonScoreRankingDdo, LeagueScoreRankingReply>();
        CreateMap<LeagueSeasonCountRankingDdo, LeagueCountRankingReply>();
        CreateMap<LeagueSeasonAverageWeightRankingDdo, LeagueAverageWeightRankingReply>();
        CreateMap<LeagueSeasonAverageTimeRankingDdo, LeagueAverageTimeRankingReply>();
        CreateMap<LeagueSeasonStreakRankingDdo, LeagueStreakRankingReply>();
        
        CreateMap<LeagueSeasonMemberEvaluationDdo, LeagueSeasonMemberEvaluationReply>();
    }
}