using Valmar.Domain.Classes;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain;

public interface ILeaguesDomainService
{
    Task<LeagueSeasonEvaluationDdo> EvaluateLeagueSeason(int year, int month);
    Task<LeagueSeasonMemberEvaluationDdo> EvaluateOwnLeagueSeason(int year, int month, int login);
}