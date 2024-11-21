using AutoMapper;
using Grpc.Core;
using tobeh.Valmar.Server.Domain;
using tobeh.Valmar.Server.Grpc.Utils;

namespace tobeh.Valmar.Server.Grpc;

public class AnnouncementsGrpcService(
    ILogger<AnnouncementsGrpcService> logger,
    IMapper mapper,
    IAnnouncementsDomainService announcementsService) : Announcements.AnnouncementsBase
{
    public override async Task GetAllAnnouncements(GetAnnouncementsMessage request,
        IServerStreamWriter<AnnouncementMessage> responseStream,
        ServerCallContext context)
    {
        logger.LogTrace("GetAllAnnouncements(request={request})", request);

        var announcements = await announcementsService.GetAnnouncements();
        await responseStream.WriteAllMappedAsync(announcements, mapper.Map<AnnouncementMessage>);
    }
}