using Valmar.Database;
using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Domain.Implementation;

public class StatsDomainService(
    ILogger<StatsDomainService> logger, 
    BubbleChunkTreeProvider bubbleChunks) : IStatsDomainService
{
    public async Task<BubbleTimespanRangeDdo> GetMemberBubblesInRange(int login, DateTimeOffset start, DateTimeOffset end)
    {
        logger.LogTrace("GetMemberBubblesInRange(login={login}, start={start}, end={end})", login, start, end);

        var chunk = bubbleChunks.GetTree().Chunk;
        var range = await chunk.GetAmountCollectedInTimespan(login, start, end);

        return new BubbleTimespanRangeDdo(range.StartAmount ?? 0, range.EndAmount ?? (range.StartAmount ?? 0));
    }
}