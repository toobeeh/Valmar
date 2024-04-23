using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class OutfitMapperProfile : Profile
{
    public OutfitMapperProfile()
    {
        CreateMap<OutfitDdo, OutfitMessage>().ReverseMap();
    }
}