using Valmar.Database;

namespace Valmar.Util.NChunkTree;

public class DropChunkLeaf : NChunkTree<IDropChunkNode>, IDropChunkNode
{
    public static List<PastDropEntity> drops;
    
    public DropChunkLeaf(long dropIndexStart, long dropIndexEnd) : base(0)
    {
        DropIndexStart = dropIndexStart;
        DropIndexEnd = dropIndexEnd;
    }
    protected override long ChunkStartIndex => DropIndexStart;
    protected override long ChunkEndIndex => DropIndexEnd;
    protected override NChunkTree<IDropChunkNode> CreateExpansionNode()
    {
        return new DropChunkTree(ChunkNumber);
    }

    protected override IDropChunkNode Chunk => this;
    
    public long DropIndexStart { get; }

    public long DropIndexEnd { get; }
    public double GetTotalDropValueForUser(string id)
    {
        var score = DropChunkLeaf.drops
            .Where(d => d.EventDropId >= DropIndexStart && d.EventDropId <= DropIndexEnd && d.CaughtLobbyPlayerId == id)
            .Sum(d => d.LeagueWeight == 0 ? 1 : Weight(d.LeagueWeight));

        return score;
    }

    private double Weight(double catchSeconds)
    {
        if (catchSeconds < 0) return 0;
        if (catchSeconds > 1000) return 30;
        return -1.78641975945623 * Math.Pow(10, -9) * Math.Pow(catchSeconds, 4) + 0.00000457264006980028 * Math.Pow(catchSeconds, 3) - 0.00397188791256729 * Math.Pow(catchSeconds, 2) + 1.21566760222325 * catchSeconds;
    }
}