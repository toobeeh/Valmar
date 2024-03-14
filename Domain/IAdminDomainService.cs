using Valmar.Domain.Classes.Param;

namespace Valmar.Domain;

public interface IAdminDomainService
{
    Task ReevaluateDropChunks();

    Task SetPermissionFlag(IList<long> userIds, int flag, bool state, bool toggleOthers);
    Task CreateBubbleTraces();
    Task ClearVolatileData();
    Task IncrementMemberBubbles(IList<int> userLogins);
    Task WriteOnlineItems(List<OnlineItemDdo> items);
}