using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes.JSON;

namespace tobeh.Valmar.Server.Mappers;

public class CardMapperProfile : Profile
{
    public CardMapperProfile()
    {
        CreateMap<CustomCardJson, MemberCardSettingsMessage>().ReverseMap();
        CreateMap<CardTemplateEntity, CardTemplateListingMessage>();
        CreateMap<CardTemplateEntity, CardTemplateMessage>();
    }
}