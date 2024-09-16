using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Domain.Classes;
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
        logger.LogTrace("SearchCloud({request})", request);

        var member = await membersDomainService.GetMemberByLogin(request.OwnerLogin);
        var tags = await cloudDomainService.SearchCloudTags(member, request.PageSize, request.Page, request.TitleQuery,
            request.AuthorQuery, request.LanguageQuery, request.CreatedBeforeQuery, request.CreatedAfterQuery,
            request.CreatedInPrivateLobbyQuery, request.IsOwnQuery);
        await responseStream.WriteAllMappedAsync(tags, mapper.Map<CloudImageMessage>);
    }

    public override async Task<CloudImageIdMessage> SaveCloudTags(CloudTagMessage request, ServerCallContext context)
    {
        logger.LogTrace("SaveCloudTags({request})", request);

        var id = await cloudDomainService.SaveCloudTags(new CloudImageTagDdo(request.Title, request.Author,
            request.Language,
            DateTimeOffset.FromUnixTimeMilliseconds(request.CreatedAt), request.CreatedInPrivateLobby, request.IsOwn,
            request.OwnerLogin));
        return new CloudImageIdMessage { Id = id };
    }

    public override async Task<CloudImageMessage> GetCloudTagsById(GetCloudTagsByIdMessage request,
        ServerCallContext context)
    {
        logger.LogTrace("GetCloudTagsById({request})", request);

        var tag = await cloudDomainService.GetCloudTagsById(request.OwnerLogin, request.Id);
        return mapper.Map<CloudImageMessage>(tag);
    }

    public override async Task<Empty> DeleteCloudTags(DeleteCloudTagsMessage request, ServerCallContext context)
    {
        logger.LogTrace("DeleteCloudTags({request})", request);

        await cloudDomainService.DeleteCloudTags(request.OwnerLogin, request.Ids);
        return new Empty();
    }

    public override async Task<Empty> LinkImageToAward(LinkImageToAwardMessage request, ServerCallContext context)
    {
        logger.LogTrace("LinkImageToAward({request})", request);

        await cloudDomainService.LinkImageToAward(request.OwnerLogin, request.Id, request.AwardInventoryId);
        return new Empty();
    }
}