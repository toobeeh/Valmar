using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;

namespace Valmar.Domain;

public interface IThemesDomainService
{
    /// <summary>
    /// Gets a theme by its share id and increments the public download counter if published and requested
    /// </summary>
    /// <param name="id">Theme share id</param>
    /// <param name="incrementDownloads">Whether the download count should be incremented, if it is a published theme</param>
    /// <returns>Theme json data unprocessed</returns>
    Task<string> GetThemeById(string id, bool incrementDownloads);
    
    /// <summary>
    /// Gets all ids of published themes
    /// </summary>
    /// <returns>List of published theme ids</returns>
    Task<List<string>> GetPublishedThemes();
    
    /// <summary>
    /// Adds a listing to the published themes for given id
    /// </summary>
    /// <param name="id">Theme share id of the theme to be published</param>
    /// <param name="owner">Name or ID of the theme creator</param>
    /// <returns></returns>
    Task PublishTheme(string id, string owner);
    
    /// <summary>
    /// Adds a theme json to the theme shares and returns its created id
    /// </summary>
    /// <param name="themeJson">Unprocessed theme json string</param>
    /// <returns>Id of the uploaded theme</returns>
    Task<string> ShareTheme(string themeJson);
    
    /// <summary>
    /// Updates the theme content of a shared theme
    /// </summary>
    /// <param name="id">Id of the shared theme</param>
    /// <param name="updateId">Id of the theme which content will be pulled and inserted to the target id. Is not modified.</param>
    /// <returns></returns>
    Task UpdateTheme(string id, string updateId);

    /// <summary>
    /// Deserialized the theme json
    /// </summary>
    /// <param name="themeJson">Theme json string</param>
    /// <returns>Theme object</returns>
    ThemeJson ParseTheme(string themeJson);
    
    /// <summary>
    /// Gets listing information of a theme
    /// </summary>
    /// <param name="id">Target theme id</param>
    /// <returns>Listing metadata</returns>
    Task<ThemeListingDdo> GetListingOfTheme(string id);
}