using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class CloudDomainService(
    ILogger<CloudDomainService> logger,
    PalantirContext db) : ICloudDomainService
{
    public async Task<List<CloudImageDdo>> SearchCloudTags(MemberDdo member, int pageSize, int page, string? title,
        string? author, string? language, long? createdbefore, long? createdAfter, bool? createdInPrivatelobby,
        bool? isOwn)
    {
        logger.LogTrace(
            "SearchCloudTags({title}, {author}, {language}, {createdbefore}, {createdAfter}, {createdInPrivatelobby}, {isOwn})",
            title, author, language, createdbefore, createdAfter, createdInPrivatelobby, isOwn);

        var results = await db.CloudTags
            .Where(tag =>
                tag.Owner == member.Login &&
                (isOwn == null || tag.Own == isOwn) &&
                (createdInPrivatelobby == null || tag.Private == createdInPrivatelobby) &&
                (title == null || tag.Title == title) &&
                (author == null || tag.Author == author) &&
                (language == null || tag.Language == language) &&
                (createdbefore == null || tag.Date < createdbefore) &&
                (createdAfter == null || tag.Date > createdAfter))
            .OrderByDescending(tag => tag.Date)
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToListAsync();


        return results.Select(image => new CloudImageDdo(
            image.ImageId,
            $"https://cloud.typo.rip/{member.DiscordId}/{image.ImageId}/image.png",
            $"https://cloud.typo.rip/{member.DiscordId}/{image.ImageId}/meta.json",
            $"https://cloud.typo.rip/{member.DiscordId}/{image.ImageId}/commands.json",
            new CloudImageTagDdo(image.Title, image.Author, image.Language,
                DateTimeOffset.FromUnixTimeMilliseconds(image.Date), image.Private, image.Own)
        )).ToList();
    }
}