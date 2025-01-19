using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class DropMapperProfile : Profile
{
    public DropMapperProfile()
    {
        CreateMap<ClaimDropResultDdo, ClaimDropResultMessage>();
    }
}