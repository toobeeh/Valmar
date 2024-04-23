using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;
using tobeh.Valmar.Server.Domain.Exceptions;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class CardDomainService(
    ILogger<CardDomainService> logger, 
    PalantirContext db) : ICardDomainService
{
    public async Task<CardTemplateEntity> GetTemplate(string name)
    {
        logger.LogTrace("GetTemplate(name={name})", name);
        
        var template = await db.CardTemplates.FirstOrDefaultAsync(entity => entity.Name == name);
        if (template is null)
        {
            throw new EntityNotFoundException($"Template with name {name} not found");
        }

        return template;
    }
    
    public async Task<List<CardTemplateEntity>> GetAllTemplates()
    {
        logger.LogTrace("GetAllTemplates()");
        
        var templates = await db.CardTemplates.ToListAsync();
        return templates;
    }
    
    public async Task<CustomCardJson> GetCustomCardSettings(MemberDdo member)
    {
        logger.LogTrace("GetCustomCardSettings(member={member})", member);
        
        var memberEntity = await db.Members.FirstAsync(entity => entity.Login == member.Login);
        var parsedSettings = member.MappedFlags.Any(flag => flag is MemberFlagDdo.Admin or MemberFlagDdo.Patron) && memberEntity.Customcard is {} cardValue ? 
            ValmarJsonParser.TryParse<CustomCardJson>(cardValue, logger, true):
            new CustomCardJson(1, 0.7, null, "white", "white", "black", "classic");
        
        if(parsedSettings.TemplateName is null) parsedSettings = parsedSettings with { TemplateName = "classic" };

        return parsedSettings;
    }
    
    public async Task SaveCustomCardSettings(MemberDdo member, CustomCardJson settings)
    {
        logger.LogTrace("SaveCustomCardSettings(member={member}, settings={settings})", member, settings);
        
        var memberEntity = await db.Members.FirstAsync(entity => entity.Login == member.Login);
        memberEntity.Customcard = JsonSerializer.Serialize(settings);
        await db.SaveChangesAsync();
    }
}