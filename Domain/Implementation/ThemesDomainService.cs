using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class ThemesDomainService(
    ILogger<ThemesDomainService> logger, 
    PalantirContext db) : IThemesDomainService
{
    
    private static string GenerateRandomId(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var randomArray = new char[length];

        for (var i = 0; i < length; i++)
        {
            randomArray[i] = chars[random.Next(chars.Length)];
        }

        return new string(randomArray);
    }

    public async Task<string> GetThemeById(string id, bool incrementDownloads = false)
    {
        logger.LogTrace("GetThemeById(id={id})", id);

        var theme = await db.ThemeShares.FirstOrDefaultAsync(t => t.Id == id);
        if (theme is null)
        {
            throw new EntityNotFoundException($"Theme with id {id} does not exist.");
        }

        if (incrementDownloads)
        {
            var publishedTheme = await db.UserThemes.FirstOrDefaultAsync(t => t.Id == id);
            if (publishedTheme is null)
            {
                throw new EntityNotFoundException($"Theme with id {id} is not published, but download increment was requested.");
            }

            publishedTheme.Downloads++;
            db.UserThemes.Update(publishedTheme);
            await db.SaveChangesAsync();
        }

        return theme.Theme;
    }

    public async Task<List<string>> GetPublishedThemes()
    {
        return await db.UserThemes.Select(t => t.Id).ToListAsync();
    }

    public async Task PublishTheme(string id, string owner)
    {
        var theme = await GetThemeById(id);
        var meta = ParseTheme(theme);

        var userTheme = new UserThemeEntity()
        {
            Downloads = 0,
            Id = id,
            Version = 1,
            OwnerId = owner
        };

        db.UserThemes.Add(userTheme);
        await db.SaveChangesAsync();
    }

    public async Task<string> ShareTheme(string themeJson)
    {
        var themeShare = new ThemeShareEntity()
        {
            Theme = themeJson,
            Id = GenerateRandomId()
        };
        db.ThemeShares.Add(themeShare);
        await db.SaveChangesAsync();

        return themeShare.Id;
    }

    public async Task UpdateTheme(string id, string updateId)
    {
        var publishedTheme = await db.UserThemes.FirstOrDefaultAsync(t => t.Id == id);
        if (publishedTheme is null)
        {
            throw new EntityNotFoundException($"Theme with id {id} is not published.");
        }

        var newTheme = await GetThemeById(updateId);
        var theme = await db.ThemeShares.FirstAsync(t => t.Id == publishedTheme.Id);
        
        theme.Theme = newTheme;
        publishedTheme.Version++;
        db.UserThemes.Update(publishedTheme);
        await db.SaveChangesAsync();
    }

    public async Task<ThemeListingDdo> GetListingOfTheme(string id)
    {
        
        // TODO improve performance by taking theme entity as argument
        
        var publishedTheme = await db.UserThemes.FirstOrDefaultAsync(t => t.Id == id);
        if (publishedTheme is null)
        {
            throw new EntityNotFoundException($"Theme with id {id} is not published.");
        }

        var theme = await GetThemeById(id, false);
        var meta = ParseTheme(theme).Meta;

        return new ThemeListingDdo(meta.Name, publishedTheme.Version, publishedTheme.Downloads, id, meta.Author);
    }

    public ThemeJson ParseTheme(string themeJson)
    {
        ThemeJson? theme = null;
        try
        {
            theme = JsonSerializer.Deserialize<ThemeJson>(themeJson, ValmarJsonOptions.JsonSerializerOptions);
        }
        catch(Exception e)
        {
            logger.LogError(e, "Failed to parse theme");
            theme = null;
        }
        
        if (theme is null)
        {
            throw new NullReferenceException($"Failed to parse theme: \n{themeJson}");
        }
        
        return theme;
    }
}