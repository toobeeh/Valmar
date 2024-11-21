using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Domain;

public interface IAnnouncementsDomainService
{
    Task<List<TypoAnnouncementEntity>> GetAnnouncements();
}