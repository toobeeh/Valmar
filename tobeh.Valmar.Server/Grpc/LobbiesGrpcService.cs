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
    public override async Task GetLobbyDropClaims(GetLobbyDropClaimsRequest request,
        IServerStreamWriter<DropLogReply> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetLobbyDropClaims(request={request})", request);

        var drops = await lobbiesService.GetLobbyDrops(request.LobbyKey);
        await responseStream.WriteAllMappedAsync(drops, mapper.Map<DropLogReply>);
    }

    public override Task<PlainLobbyLinkMessage> DecryptLobbyLinkToken(EncryptedLobbyLinkTokenMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("DecryptLobbyLinkToken(request={request})", request);

        var plain = LobbiesHelper.DecryptLobbyLink(request.Token);
        return Task.FromResult(new PlainLobbyLinkMessage { Link = plain.Link, GuildId = plain.GuildId });
    }

    public override Task<EncryptedLobbyLinkTokenMessage> EncryptLobbyLinkToken(PlainLobbyLinkMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("EncryptLobbyLinkToken(request={request})", request);

        return Task.FromResult(new EncryptedLobbyLinkTokenMessage
            { Token = LobbiesHelper.EncryptLobbyLink(new PlainLobbyLinkDdo(request.Link, request.GuildId)) });
    }

    public override async Task<SkribblLobbyTypoSettingsMessage> GetSkribblLobbyTypoSettings(
        SkribblLobbyIdentificationMessage request, ServerCallContext context)
    {
        logger.LogTrace("GetSkribblLobby(request={request})", request);

        var lobby = await lobbiesService.GetSkribblLobbyTypoSettings(request.Link);
        return mapper.Map<SkribblLobbyTypoSettingsMessage>(lobby);
    }

    public override async Task<Empty> SetSkribblLobbyTypoSettings(SkribblLobbyTypoSettingsMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("SaveSkribblLobbyTypoSettings(request={request})", request);

        await lobbiesService.SetSkribblLobbyTypoSettings(request.LobbyId, request.LobbyOwnershipClaim,
            request.Description, request.WhitelistAllowedServers, request.AllowedServers);

        return new Empty();
    }

    public override async Task<Empty> SetMemberStatusesInLobby(SkribblLobbyTypoMembersMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("SetMemberStatusesInSkribblLobby(request={request})", request);

        await lobbiesService.SetMemberStatusesInSkribblLobby(request.LobbyId,
            mapper.Map<List<SkribblLobbyTypoMemberDdo>>(request.Members));

        return new Empty();
    }

    public override async Task<Empty> SetSkribblLobbyState(SkribblLobbyStateMessage request, ServerCallContext context)
    {
        logger.LogTrace("SetSkribblLobbyState(request={request})", request);

        await lobbiesService.SetSkribblLobbyState(request.LobbyId, mapper.Map<SkribblLobbyStateDdo>(request));
        return new Empty();
    }

    public override async Task<Empty> RemoveMemberStatusesInLobby(SkribblLobbyTypoMembersMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("RemoveMemberStatusesInSkribblLobby(request={request})", request);

        await lobbiesService.RemoveMemberStatusesInSkribblLobby(request.LobbyId,
            request.Members.Select(mapper.Map<SkribblLobbyTypoMemberDdo>).ToList());

        return new Empty();
    }

    public override async Task GetOnlineLobbyPlayers(GetOnlinePlayersRequest request,
        IServerStreamWriter<SkribblLobbyTypoMembersMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetOnlineLobbyPlayers(request={request})", request);

        var players = await lobbiesService.GetOnlineLobbyPlayers(request.GuildId);
        logger.LogDebug("Found {count} online players for request {request}", players.Count, request);
        await responseStream.WriteAllMappedAsync(players, mapper.Map<SkribblLobbyTypoMembersMessage>);
    }

    public override async Task<SkribblLobbyMessage> GetLobbyById(GetLobbyByIdRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetLobbyById(request={request})", request);

        var lobby = await lobbiesService.GetLobbyById(request.LobbyId);
        return mapper.Map<SkribblLobbyMessage>(lobby);
    }

    public override async Task GetLobbiesById(GetLobbiesByIdRequest request,
        IServerStreamWriter<SkribblLobbyMessage> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetLobbiesById(request={request})", request);

        var lobbies = await lobbiesService.GetLobbiesById(request.LobbyIds.ToList());
        logger.LogDebug("Found {count} lobbies", lobbies.Count);
        await responseStream.WriteAllMappedAsync(lobbies,
            lobby => Task.FromResult(mapper.Map<SkribblLobbyMessage>(lobby)), true);
    }

    public override async Task<Empty> ClearOrphanedLobbyData(Empty request, ServerCallContext context)
    {
        logger.LogTrace("ClearOrphanedLobbyData(request={request})", request);

        await lobbiesService.ClearOrphanedLobbyData();
        return new Empty();
    }
}