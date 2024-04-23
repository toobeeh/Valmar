using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;

namespace tobeh.Valmar.Server.Domain;

public interface ICardDomainService
{
    Task<CustomCardJson> GetCustomCardSettings(MemberDdo member);
    Task<CardTemplateEntity> GetTemplate(string name);
    Task<List<CardTemplateEntity>> GetAllTemplates();
    Task SaveCustomCardSettings(MemberDdo member, CustomCardJson settings);
}