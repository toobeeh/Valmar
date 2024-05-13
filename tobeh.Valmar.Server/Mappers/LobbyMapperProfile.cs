using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;

namespace tobeh.Valmar.Server.Mappers;

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
            .ForMember(dest => dest.Players, opt => opt.Ignore()) // ignore ambigious property name
            .ForMember(
                dest => dest.SkribblDetails,
                opt => opt.MapFrom(src => src));
        CreateMap<List<PalantirLobbyPlayerDdo>, LobbyReply>().ForMember(
            dest => dest.Players,
            opt => opt.MapFrom(src => src));

        // mappings for drops
        CreateMap<PastDropEntity, DropLogReply>().ConvertUsing(drop => MapDropEntity(drop));

        // mappings for onlinemembers
        CreateMap<OnlineMemberDdo, OnlineMemberReply>();
        CreateMap<JoinedLobbyDdo, JoinedLobbyMessage>();

        CreateMap<LobbyLinkDdo, GuildLobbyLinkMessage>().ReverseMap();
    }

    private DropLogReply MapDropEntity(PastDropEntity drop)
    {
        var d = new DropLogReply()
        {
            Id = drop.DropId,
            ClaimDiscordId = Convert.ToInt64(drop.CaughtLobbyPlayerId),
            LobbyKey = drop.CaughtLobbyKey,
            ValidFrom = drop.ValidFrom,
            EventDropId = drop.EventDropId == 0 ? null : drop.EventDropId,
            LeagueTime = drop.LeagueWeight == 0 ? null : drop.LeagueWeight
        };

        return d;
    }
}