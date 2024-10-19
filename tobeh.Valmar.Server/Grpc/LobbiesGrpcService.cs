using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Grpc.Utils;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Grpc;

public class LobbiesGrpcService(
    ILogger<LobbiesGrpcService> logger,
    IMapper mapper,
    ILobbiesDomainService lobbiesService) : Lobbies.LobbiesBase
{
    public override async Task GetCurrentLobbies(Empty request, IServerStreamWriter<LobbyReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace($"GetCurrentLobbies(empty)");

        // get all current lobbies' skribbl details
        var lobbies = await lobbiesService.GetPalantirLobbies();

        // map lobbies with palantir details and players
        await responseStream.WriteAllMappedAsync(lobbies, async palantirLobby =>
        {
            var skribblLobby = await lobbiesService.GetSkribblLobbyDetails(palantirLobby.Id);
            var players = await lobbiesService.GetPalantirLobbyPlayers(palantirLobby, skribblLobby);

            // execute multiple mappings on automapper to one object
            var response = mapper
                .Map<LobbyReply>(palantirLobby)
                .Map(skribblLobby, mapper)
                .Map(players, mapper);

            return response;
        }, true);
    }

    public override async Task GetLobbyDropClaims(GetLobbyDropClaimsRequest request,
        IServerStreamWriter<DropLogReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetLobbyDropClaims(request={request})", request);

        var drops = await lobbiesService.GetLobbyDrops(request.LobbyKey);
        await responseStream.WriteAllMappedAsync(drops, mapper.Map<DropLogReply>);
    }

    public override async Task GetOnlinePlayers(Empty request, IServerStreamWriter<OnlineMemberReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetOnlinePlayers(request={request})", request);

        var members = await lobbiesService.GetOnlineMembers();
        await responseStream.WriteAllMappedAsync(members, mapper.Map<OnlineMemberReply>);
    }

    public override async Task<Empty> SetGuildLobbyLinks(SetGuildLobbyLinksMessage request, ServerCallContext context)
    {
        logger.LogTrace("SetGuildLobbyLinks(request={request})", request);

        await lobbiesService.SetGuildLobbyLinks(request.GuildId, mapper.Map<List<LobbyLinkDdo>>(request.Links));
        return new Empty();
    }

    public override Task<PlainLobbyLinkMessage> DecryptLobbyLinkToken(EncryptedLobbyLinkTokenMessage request,
        ServerCallContext context)
    {
        var plain = LobbiesHelper.DecryptLobbyLink(request.Token);
        return Task.FromResult(new PlainLobbyLinkMessage { Link = plain.Link, GuildId = plain.GuildId });
    }

    public override Task<EncryptedLobbyLinkTokenMessage> EncryptLobbyLinkToken(PlainLobbyLinkMessage request,
        ServerCallContext context)
    {
        return Task.FromResult(new EncryptedLobbyLinkTokenMessage
            { Token = LobbiesHelper.EncryptLobbyLink(new PlainLobbyLinkDdo(request.Link, request.GuildId)) });
    }

    public override async Task GetLobbyLinks(Empty request, IServerStreamWriter<GuildLobbyLinkMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetLobbyLinks(request={request})", request);

        var links = await lobbiesService.GetGuildLobbyLinks();
        await responseStream.WriteAllMappedAsync(links, mapper.Map<GuildLobbyLinkMessage>);
    }
}