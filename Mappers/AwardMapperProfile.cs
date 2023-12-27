using AutoMapper;
using Valmar.Database;

namespace Valmar.Mappers;

public class AwardMapperProfile : Profile
{
    public AwardMapperProfile()
    {
        CreateMap<AwardEntity, AwardReply>();
    }
}