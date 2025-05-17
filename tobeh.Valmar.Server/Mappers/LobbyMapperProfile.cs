using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class LobbyMapperProfile : Profile
{
    public LobbyMapperProfile()
    {
        // mappings for nested lobby details
        CreateMap<PlainLobbyLinkDdo, PlainLobbyLinkMessage>().ReverseMap();

        CreateMap<SkribblLobbyTypoMemberDdo, SkribblLobbyTypoMemberMessage>().ReverseMap();
        CreateMap<SkribblLobbyTypoMembersDdo, SkribblLobbyTypoMembersMessage>().ReverseMap();
        CreateMap<SkribblLobbyStateDdo, SkribblLobbyStateMessage>().ReverseMap();
        CreateMap<SkribblLobbyDdo, SkribblLobbyMessage>();
        CreateMap<SkribblLobbyTypoSettingsDdo, SkribblLobbyTypoSettingsMessage>().ReverseMap();
        CreateMap<SkribblLobbySkribblPlayerDdo, SkribblLobbySkribblPlayerMessage>().ReverseMap();
        CreateMap<SkribblLobbySkribblSettingsDdo, SkribblLobbySkribblSettingsMessage>().ReverseMap();


        // mappings for drops
        CreateMap<PastDropEntity, DropLogReply>().ConvertUsing(drop => MapDropEntity(drop));

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