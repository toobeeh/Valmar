using AutoMapper;
using tobeh.Valmar.Server.Database;
using AutoMapper.Extensions.EnumMapping;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class MemberMapperProfile : Profile
{
    public MemberMapperProfile()
    {
        CreateMap<MemberDdo, MemberReply>();
        CreateMap<MemberFlagDdo, MemberFlagMessage>()
            .ConvertUsingEnumMapping(opt => opt.MapByName());
        CreateMap<MemberSearchDdo, MemberSearchReply>()
            .ForPath(
                dest => dest.Raw.MemberJson, 
                opt => opt.MapFrom((src => src.MemberJson)));
    }
}