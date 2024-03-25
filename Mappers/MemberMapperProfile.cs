using AutoMapper;
using Valmar.Database;
using AutoMapper.Extensions.EnumMapping;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

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