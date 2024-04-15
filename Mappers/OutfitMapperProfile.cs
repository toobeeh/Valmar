using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class OutfitMapperProfile : Profile
{
    public OutfitMapperProfile()
    {
        CreateMap<OutfitDdo, OutfitMessage>();
    }
}