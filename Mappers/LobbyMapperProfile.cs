using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;

namespace Valmar.Mappers;

public class LobbyMapperProfile : Profile
{
    public LobbyMapperProfile()
    {
        
        // mappings for nested lobby details
        CreateMap<PalantirLobbyJson, PalantirLobbyDetails>();
        CreateMap<SkribblLobbyReportJson, SkribblLobbyDetails>();
        CreateMap<SkribblLobbyPlayerReportJson, SkribblLobbyPlayer>();
        CreateMap<PalantirLobbyPlayerDdo, PalantirLobbyPlayer>();
        
        // mappings for partial mapping of lobby reply
        CreateMap<PalantirLobbyJson, LobbyReply>().ForMember(
            dest => dest.PalantirDetails, 
            opt => opt.MapFrom(src => src));
        CreateMap<SkribblLobbyReportJson, LobbyReply>()
            .ForMember(dest=>dest.Players, opt => opt.Ignore()) // ignore ambigious property name
            .ForMember(
            dest => dest.SkribblDetails, 
            opt => opt.MapFrom(src => src));
        CreateMap<List<PalantirLobbyPlayerDdo>, LobbyReply>().ForMember(
            dest => dest.Players, 
            opt => opt.MapFrom(src => src));

        // mappings for drops
        CreateMap<PastDropEntity, DropLogReply>().ConvertUsing(drop => MapDropEntity(drop));
        
        // mappings for onlinemembers
        CreateMap<OnlineMemberDdo, OnlineMemberReply>()
            .ForMember(dest => dest.JoinedLobbies,
                opt => opt.MapFrom(src => src.JoinedLobbies));
    }

    private DropLogReply MapDropEntity(PastDropEntity drop)
    {
        var d = new DropLogReply()
        {
            Id = drop.DropId,
            ClaimDiscordId = Convert.ToInt64(drop.CaughtLobbyPlayerId),
            LobbyKey = drop.CaughtLobbyKey,
            ValidFrom = drop.ValidFrom,
            EventDropId =  drop.EventDropId == 0 ? null : drop.EventDropId,
            LeagueTime = drop.LeagueWeight == 0 ? null : drop.LeagueWeight
        };

        return d;
    }
}