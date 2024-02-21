using System.Diagnostics;

namespace Valmar.Util.NChunkTree;

public class PersistentDropChunk(long dropIndexStart, long dropIndexEnd) : IDropChunk
{
    public long? DropIndexStart => dropIndexStart;
    public long? DropIndexEnd => dropIndexEnd;
    
    public Task<double> GetTotalDropValueForUser(string id)
    {
        var score = DropChunkLeaf.drops
            .Where(d => (DropIndexStart is null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd is null || d.DropId <= DropIndexEnd) 
                        && d.CaughtLobbyPlayerId == id)
            .Sum(d => d.LeagueWeight == 0 ? 1 : Weight(d.LeagueWeight));

        return Task.FromResult(score);
    }

    private double Weight(double catchMs)
    {
        if (catchMs < 0) return 0;
        if (catchMs > 1000) return 30;
        var weight =  -1.78641975945623 * Math.Pow(10, -9) * Math.Pow(catchMs, 4) + 0.00000457264006980028 * Math.Pow(catchMs, 3) - 0.00397188791256729 * Math.Pow(catchMs, 2) + 1.21566760222325 * catchMs;
        return weight / 100;
    }
}