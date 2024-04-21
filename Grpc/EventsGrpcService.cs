using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Valmar.Domain;

namespace Valmar.Grpc;

public class EventsGrpcService(
    ILogger<EventsGrpcService> logger, 
    IMapper mapper,
    IEventsDomainService eventsService) : Events.EventsBase 
{
    public override async Task GetAllEvents(Empty request, IServerStreamWriter<EventReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetAllEvents(empty)");
        
        var events = await eventsService.GetAllEvents();
        foreach (var eventEntity in events)
        {
            await responseStream.WriteAsync(mapper.Map<EventReply>(eventEntity));
        }
    }

    public override async Task<EventReply> GetCurrentEvent(Empty request, ServerCallContext context)
    {
        logger.LogTrace("GetCurrentEvent(empty)");
        
        var eventEntity = await eventsService.GetCurrentEvent();
        return mapper.Map<EventReply>(eventEntity);
    }

    public override async Task<EventReply> GetEventById(GetEventRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetEventById(request={request})", request);
        
        var eventEntity = await eventsService.GetEventById(request.Id);
        return mapper.Map<EventReply>(eventEntity);
    }
    
    public override async Task GetAllEventDrops(Empty request, IServerStreamWriter<EventDropReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetAllEventDrops(empty)");
        
        var eventDrops = await eventsService.GetEventDrops();
        foreach (var eventDrop in eventDrops)
        {
            await responseStream.WriteAsync(mapper.Map<EventDropReply>(eventDrop));
        }
    }

    public override async Task<EventDropReply> GetEventDropById(GetEventDropRequest request, ServerCallContext context)
    {
        logger.LogTrace("GetEventDropById(request={request})", request);
        
        var eventDrop = await eventsService.GetEventDropById(request.Id);
        return mapper.Map<EventDropReply>(eventDrop);
    }
    
    public override async Task GetEventDropsOfEvent(GetEventRequest request, IServerStreamWriter<EventDropReply> responseStream, ServerCallContext context)
    {
        logger.LogTrace("GetEventDropsOfEvent(request={request})", request);
        
        var eventDrops = await eventsService.GetEventDrops(request.Id);
        foreach (var eventDrop in eventDrops)
        {
            await responseStream.WriteAsync(mapper.Map<EventDropReply>(eventDrop));
        }
    }
}