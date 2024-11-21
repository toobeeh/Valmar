using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class AnnouncementsDomainService(
    ILogger<AnnouncementsDomainService> logger,
    PalantirContext db) : IAnnouncementsDomainService
{
    public async Task<List<TypoAnnouncementEntity>> GetAnnouncements()
    {
        logger.LogTrace("GetAnnouncementsMessage()");

        return await db.TypoAnnouncements.ToListAsync();
    }
}