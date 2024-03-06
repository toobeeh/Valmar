namespace Valmar.Domain;

public interface IAdminDomainService
{
    Task ReevaluateDropChunks();

    Task SetPermissionFlag(IList<long> userIds, int flag, bool state, bool toggleOthers);
}