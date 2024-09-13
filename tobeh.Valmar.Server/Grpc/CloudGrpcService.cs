using AutoMapper;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class CloudGrpcService(
    ILogger<CloudGrpcService> logger,
    IMapper mapper,
    IMembersDomainService membersDomainService,
    ICloudDomainService cloudDomainService) : Cloud.CloudBase
{
    public override async Task SearchCloud(SearchCloudMessage request,
        IServerStreamWriter<CloudImageMessage> responseStream, ServerCallContext context)
    {
        var member = await membersDomainService.GetMemberByLogin(request.OwnerLogin);
        var tags = await cloudDomainService.SearchCloudTags(member, request.PageSize, request.Page, request.TitleQuery,
            request.AuthorQuery, request.LanguageQuery, request.CreatedBeforeQuery, request.CreatedAfterQuery,
            request.CreatedInPrivateLobbyQuery, request.IsOwnQuery);
        await responseStream.WriteAllMappedAsync(tags, mapper.Map<CloudImageMessage>);
    }
}