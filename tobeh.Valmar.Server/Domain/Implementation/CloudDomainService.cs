using Microsoft.EntityFrameworkCore;
using SnowflakeGenerator;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;

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
                DateTimeOffset.FromUnixTimeMilliseconds(image.Date), image.Private, image.Own, image.Owner)
        )).ToList();
    }

    public async Task<CloudImageDdo> GetCloudTagsById(int ownerLogin, long id)
    {
        logger.LogTrace("GetCloudTagsById({login}, {id})", ownerLogin, id);

        var tag = await db.CloudTags.FirstAsync(tag => tag.Owner == ownerLogin && tag.ImageId == id);
        if (tag is null)
        {
            throw new EntityNotFoundException($"No cloud tags found with id {id}");
        }

        return MapToDdo(tag);
    }

    public async Task DeleteCloudTags(int ownerLogin, IEnumerable<long> ids)
    {
        logger.LogTrace("DeleteCloudTags({login}, {ids})", ownerLogin, ids);

        var tags = await db.CloudTags.Where(tag => tag.Owner == ownerLogin && ids.Contains(tag.ImageId)).ToListAsync();
        if (tags.Count != ids.Count())
        {
            throw new EntityNotFoundException("Some given cloud tags were not found");
        }

        db.CloudTags.RemoveRange(tags);
        await db.SaveChangesAsync();
    }

    public async Task<long> SaveCloudTags(CloudImageTagDdo tags)
    {
        logger.LogTrace("SaveCloudTags({tags})", tags);

        var tag = new CloudTagEntity
        {
            Title = tags.Title,
            Author = tags.Author,
            Language = tags.Language,
            Date = tags.CreatedAt.ToUnixTimeMilliseconds(),
            Private = tags.CreatedInPrivateLobby,
            Own = tags.IsOwn,
            Owner = tags.OwnerLogin,
            ImageId = new Snowflake().NextID()
        };

        await db.CloudTags.AddAsync(tag);
        await db.SaveChangesAsync();
        return tag.ImageId;
    }

    public async Task LinkImageToAward(int ownerLogin, long imageId, int awardId)
    {
        logger.LogTrace("LinkImageToAward({ownerLogin}, {imageId}, {awardId})", ownerLogin, imageId, awardId);

        var award = await db.Awardees.FirstAsync(award => award.AwardeeLogin == ownerLogin && award.Id == awardId);
        if (award is null)
        {
            throw new EntityNotFoundException($"No award found with id {awardId} in inventory of {ownerLogin}");
        }

        if (award.ImageId != null)
        {
            throw new EntityAlreadyExistsException(
                $"Award {awardId} in inventory of {ownerLogin} already has an image linked");
        }

        award.ImageId = imageId;
        db.Awardees.Update(award);

        await db.SaveChangesAsync();
    }

    private CloudImageDdo MapToDdo(CloudTagEntity tag)
    {
        return new CloudImageDdo(
            tag.ImageId,
            $"https://cloud.typo.rip/{tag.Owner}/{tag.ImageId}/image.png",
            $"https://cloud.typo.rip/{tag.Owner}/{tag.ImageId}/meta.json",
            $"https://cloud.typo.rip/{tag.Owner}/{tag.ImageId}/commands.json",
            new CloudImageTagDdo(tag.Title, tag.Author, tag.Language,
                DateTimeOffset.FromUnixTimeMilliseconds(tag.Date), tag.Private, tag.Own, tag.Owner)
        );
    }
}