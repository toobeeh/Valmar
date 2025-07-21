using AutoMapper;
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

    public override async Task<JwtMessage> CreateJwt(JwtParametersMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("CreateJwt(request: {Request})", request);

        var jwt = await authorizationService.CreateJwt(
            request.TypoId,
            request.ApplicationName,
            request.Expiry.ToDateTime(),
            request.Scopes.ToList(), request.RedirectUri);
        var token = authorizationService.GetJwtString(jwt);

        var response = new JwtMessage()
        {
            Jwt = token,
            TypoId = request.TypoId
        };

        return response;
    }

    public override async Task<JwtMessage> CreateJwtForVerifiedApplication(JwtVerifiedParametersMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("CreateJwtForVerifiedApplication(request: {Request})", request);

        var jwt = await authorizationService.CreateJwtForVerifiedApplication(
            request.TypoId,
            request.ApplicationId);
        var token = authorizationService.GetJwtString(jwt);

        var response = new JwtMessage()
        {
            Jwt = token,
            TypoId = request.TypoId
        };

        return response;
    }
}