using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;
using Valmar.Domain.Exceptions;
using Valmar.Domain.Implementation;
using Valmar.Grpc.Utils;
using Valmar.Util;

namespace Valmar.Grpc;

public class MembersGrpcService(
    ILogger<MembersGrpcService> logger, 
    IMapper mapper,
    IMembersDomainService membersService) : Members.MembersBase
{
    public override async Task<MemberReply> GetMemberByAccessToken(IdentifyMemberByAccessTokenRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetMemberByAccessToken(request={request})", request);

        var member = await membersService.GetMemberByAccessToken(request.AccessToken);
        return mapper.Map<MemberReply>(member);
    }

    public override async Task<MemberReply> CreateNewMember(CreateNewMemberRequest request, ServerCallContext context)
    {
        logger.LogTrace("CreateNewMember(request={request})", request);

        var member = await membersService.CreateMember(request.DiscordId, request.Username, request.ConnectToTypoServer);
        return mapper.Map<MemberReply>(member);
    }

    public override async Task<MemberReply> GetMemberByLogin(IdentifyMemberByLoginRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetMemberByLogin(request={request})", request);

        var member = await membersService.GetMemberByLogin(request.Login);
        return mapper.Map<MemberReply>(member);
    }

    public override async Task<MemberReply> GetMemberByDiscordId(IdentifyMemberByDiscordIdRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetMemberByDiscordId(request={request})", request);

        var member = await membersService.GetMemberByDiscordId(request.Id);
        return mapper.Map<MemberReply>(member);
    }

    public override async Task<MemberReply> GetPatronizedOfMember(IdentifyMemberByDiscordIdRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetPatronizedOfMember(request={request})", request);

        var member = await membersService.GetPatronizedMemberOfPatronizer(request.Id);
        return mapper.Map<MemberReply>(member);
    }

    public override async Task SearchMember(SearchMemberRequest request, IServerStreamWriter<MemberSearchReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("SearchMember(request={request})", request);

        var members = await membersService.SearchMember(request.Query);
        await responseStream.WriteAllMappedAsync(members, mapper.Map<MemberSearchReply>);
    }

    public override async Task<RawMemberReply> GetRawMemberByLogin(IdentifyMemberByLoginRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetRawMemberByLogin(request={request})", request);

        var member = await membersService.GetRawMemberByLogin(request.Login);
        return new() {MemberJson = member};
    }

    public override async Task<AccessTokenReply> GetAccessTokenByLogin(IdentifyMemberByLoginRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetAccessTokenByLogin(request={request})", request);

        var token = await membersService.GetAccessTokenByLogin(request.Login);
        return new() { AccessToken = token };
    }

    public override async Task<MemberReply> UpdateMemberDiscordId(UpdateDiscordIdRequest request, ServerCallContext context)
    {
        logger.LogTrace("UpdateMemberDiscordId(request={request})", request);

        await membersService.UpdateMemberDiscordId(request.Login, request.DiscordId);
        var newMember = await membersService.GetMemberByLogin(request.Login);
        return mapper.Map<MemberReply>(newMember);
    }

    public override async Task<Empty> ClearMemberDropboost(IdentifyMemberByLoginRequest request, ServerCallContext context)
    {
        logger.LogTrace("ClearMemberDropboost(request={request})", request);

        await membersService.ClearMemberDropboost(request.Login);
        return new();
    }

    public override async Task<Empty> AddMemberServerConnection(ModifyServerConnectionRequest request, ServerCallContext context)
    {
        logger.LogTrace("AddMemberServerConnection(request={request})", request);

        await membersService.ConnectToServer(request.Login, request.ServerToken);
        return new();
    }

    public override async Task<Empty> RemoveMemberServerConnection(ModifyServerConnectionRequest request, ServerCallContext context)
    {
        logger.LogTrace("RemoveMemberServerConnection(request={request})", request);

        await membersService.DisconnectFromServer(request.Login, request.ServerToken);
        return new();
    }
}