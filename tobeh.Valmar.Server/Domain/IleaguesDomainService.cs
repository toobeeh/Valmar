using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util.NChunkTree.Drops;

namespace tobeh.Valmar.Server.Domain;

public interface ILeaguesDomainService
{
    Task<LeagueSeasonEvaluationDdo> EvaluateLeagueSeason(int year, int month);
    Task<LeagueSeasonMemberEvaluationDdo> EvaluateOwnLeagueSeason(int year, int month, int login);
}