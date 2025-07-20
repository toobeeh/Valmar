using AutoMapper;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes;
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

    public override async Task<ScopeRequestSignatureMessage> SignScopeRequest(SignScopeRequestMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("SignScopeRequest(request: {Request})", request);

        var signature = await authorizationService.SignScopeRequest(mapper.Map<ScopeRequestDdo>(request));
        var response = new ScopeRequestSignatureMessage
        {
            Signature = signature,
            Request = request
        };

        return response;
    }

    public override async Task<AuthorizedJwtMessage> AuthorizeJwt(ScopeRequestSignatureMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("AuthorizeJwt(request: {Request})", request);

        var scopeRequest = mapper.Map<ScopeRequestDdo>(request.Request);
        var jwt = await authorizationService.CreateJwt(scopeRequest, request.Signature);
        var token = authorizationService.GetJwtString(jwt);

        var response = new AuthorizedJwtMessage
        {
            Jwt = token,
            TypoId = request.Request.TypoId
        };

        return response;
    }
}