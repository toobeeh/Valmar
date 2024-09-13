using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class CloudMapperProfile : Profile
{
    public CloudMapperProfile()
    {
        CreateMap<CloudImageDdo, CloudImageMessage>();
        CreateMap<CloudImageTagDdo, CloudTagMessage>();
    }
}