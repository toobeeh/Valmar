using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class LobbiesGrpcService(
    ILogger<LobbiesGrpcService> logger, 
    IMapper mapper,
    ILobbiesDomainService lobbiesService) : Lobbies.LobbiesBase
{
    public override async Task GetCurrentLobbies(Empty request, IServerStreamWriter<LobbyReply> responseStream, ServerCallContext context)
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
        });
    }

    public override async Task GetCurrentGuildLobbies(GetGuildLobbiesMessage request, IServerStreamWriter<LobbyReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetCurrentGuildLobbies(request={request})", request);
    }

    public override async Task GetLobbyDropClaims(GetLobbyDropClaimsRequest request, IServerStreamWriter<DropLogReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetLobbyDropClaims(request={request})", request);

        var drops = await lobbiesService.GetLobbyDrops(request.LobbyKey);
        await responseStream.WriteAllMappedAsync(drops, mapper.Map<DropLogReply>);
    }

    public override async Task GetOnlinePlayers(Empty request, IServerStreamWriter<OnlineMemberReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetOnlinePlayers(empty)");

        var members = await lobbiesService.GetOnlineMembers();
        await responseStream.WriteAllMappedAsync(members, mapper.Map<OnlineMemberReply>);
    }
}