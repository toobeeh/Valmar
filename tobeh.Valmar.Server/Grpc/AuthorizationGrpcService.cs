using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class AuthorizationGrpcService(
    ILogger<AuthorizationGrpcService> logger,
    IAuthorizationDomainService authorizationService,
    IMapper mapper
) : Authorization.AuthorizationBase
{
    public override async Task GetAvailableScopes(GetAvailableScopesMessage request,
        IServerStreamWriter<ScopeMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetAvailableScopes(request: {Request})", request);

        var scopes = await authorizationService.GetAvailableScopes();
        await responseStream.WriteAllMappedAsync(scopes, mapper.Map<ScopeMessage>);
    }

    public override async Task<OAuth2AuthorizationCodeMessage> CreateOAuth2AuthorizationCode(
        CreateOAuth2AuthorizationCodeMessage request, ServerCallContext context)
    {
        logger.LogTrace("CreateOAuth2AuthorizationCode(request: {Request})", request);

        var code = await authorizationService.CreateOAuth2AuthorizationCode(request.TypoId, request.Oauth2ClientId);
        return mapper.Map<OAuth2AuthorizationCodeMessage>(code);
    }

    public override async Task<OAuth2AccessTokenMessage> ExchangeOauth2AuthorizationCode(
        OAuth2AuthorizationCodeMessage request, ServerCallContext context)
    {
        logger.LogTrace("ExchangeOauth2AuthorizationCode(request: {Request})", request);

        var jwt = await authorizationService.ExchangeOauth2AuthorizationCode(
            request.Oauth2AuthorizationCode, request.Oauth2ClientId, request.RedirectUri);

        return new OAuth2AccessTokenMessage
        {
            Jwt = jwt
        };
    }

    public override async Task<OAuth2ClientMessage> CreateOauth2Client(CreateOAuth2ClientMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("CreateOauth2Client(request: {Request})", request);

        var client = await authorizationService.CreateOauth2Client(
            request.Name,
            request.RedirectUri,
            request.Scopes.ToList(),
            request.OwnerTypoId);

        return mapper.Map<OAuth2ClientMessage>(client);
    }

    public override async Task GetOauth2Clients(Empty request, IServerStreamWriter<OAuth2ClientMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetOauth2Clients(request: {Request})", request);

        var clients = await authorizationService.GetOauth2Clients();
        await responseStream.WriteAllMappedAsync(clients, mapper.Map<OAuth2ClientMessage>);
    }
}