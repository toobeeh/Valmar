using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Exceptions;
using Valmar.Util;

namespace Valmar.Domain.Implementation;

public class EventsDomainService(
    ILogger<EventsDomainService> logger, 
    PalantirContext db) : IEventsDomainService
{
    private static EventDdo ConvertEventEntityToDdo(EventEntity evt)
    {
        var start = EventHelper.ParseEventTimestamp(evt.ValidFrom);
        return new EventDdo(evt.EventName, evt.EventId, evt.Description, evt.DayLength, evt.Progressive == 1, start,
            start.AddDays(evt.DayLength));
    }
    
    public async Task<EventDdo> GetEventById(int id)
    {
        logger.LogTrace("GetEventById(id={id})", id);
        
        var evt = await db.Events.FirstOrDefaultAsync(evt => evt.EventId == id);
        if (evt is null)
        {
            throw new EntityNotFoundException($"Event with id {id} does not exist.");
        }

        return ConvertEventEntityToDdo(evt);
    }
    
    public async Task<EventDdo> GetCurrentEvent()
    {
        logger.LogTrace("GetCurrentEvent()");
        
        var events = await db.Events.ToListAsync();
        var active = events
            .FirstOrDefault(evt => EventHelper.ParseEventTimestamp(evt.ValidFrom) <= DateTimeOffset.Now &&
                                   EventHelper.ParseEventTimestamp(evt.ValidFrom).AddDays(evt.DayLength) >= DateTimeOffset.Now);
        
        if (active is null)
        {
            throw new EntityNotFoundException($"No event is currently active.");
        }
        
        return ConvertEventEntityToDdo(active);
    }

    public async Task<List<EventDdo>> GetAllEvents()
    {
        return await db.Events.Select(evt => ConvertEventEntityToDdo(evt)).ToListAsync();
    }

    public async Task<EventDropEntity> GetEventDropById(int id)
    {
        logger.LogTrace("GetEventDropById(id={id})", id);
        
        var eventDrop = await db.EventDrops.FirstOrDefaultAsync(drop => drop.EventDropId == id);
        if (eventDrop is null)
        {
            throw new EntityNotFoundException($"Event drop with id {id} does not exist.");
        }

        return eventDrop;
    }

    public async Task<List<EventDropEntity>> GetAllEventDrops()
    {
        return await db.EventDrops.Where(drop => drop.EventId != 1000).ToListAsync(); // filter out placeholder drop
    }

    public async Task<List<EventDropEntity>> GetEventDropsOfEvent(int id)
    {
        logger.LogTrace("GetEventDropsOfEvent(id={id})", id);

        var evt = await GetEventById(id); // validate event exists
        return await db.EventDrops.Where(drop => drop.EventId == id).ToListAsync();
    }
}