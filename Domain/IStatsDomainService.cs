using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Domain;

public interface IStatsDomainService
{
    Task<BubbleTimespanRangeDdo> GetMemberBubblesInRange(int login, DateTimeOffset start, DateTimeOffset end);
}