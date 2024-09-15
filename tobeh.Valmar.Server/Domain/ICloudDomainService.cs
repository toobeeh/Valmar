using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface ICloudDomainService
{
    Task<List<CloudImageDdo>> SearchCloudTags(MemberDdo member, int pageSize, int page, string? title, string? author,
        string? language, long? createdbefore, long? createdAfter, bool? createdInPrivatelobby, bool? isOwn);

    Task<long> SaveCloudTags(CloudImageTagDdo tags);
    Task<CloudImageDdo> GetCloudTagsById(int ownerLogin, long id);
    Task DeleteCloudTags(int ownerLogin, IEnumerable<long> ids);
}