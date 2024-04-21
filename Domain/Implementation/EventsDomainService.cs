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

    public async Task<EventDropDdo> GetEventDropById(int id)
    {
        logger.LogTrace("GetEventDropById(id={id})", id);
        
        var eventDrop = await db.EventDrops.FirstOrDefaultAsync(drop => drop.EventDropId == id);
        if (eventDrop is null)
        {
            throw new EntityNotFoundException($"Event drop with id {id} does not exist.");
        }

        DateTimeOffset releaseStart, releaseEnd;
        var evt = await GetEventById(eventDrop.EventId);
        if (evt.Progressive)
        {
            var dropIds = await db.EventDrops
                .Where(drop => drop.EventId == evt.Id)
                .Select(drop => drop.EventDropId)
                .OrderBy(drop => drop)
                .ToListAsync();
            var releaseSlots = EventHelper.GetProgressiveEventDropReleaseSlots(evt.StartDate, evt.Length, dropIds);
            
            var slot = releaseSlots.First(slot => slot.EventDropId == eventDrop.EventDropId);
            releaseStart = slot.Start;
            releaseEnd = slot.End;
        }
        else
        {
            releaseStart = evt.StartDate;
            releaseEnd = evt.EndDate;
        }

        return new EventDropDdo(eventDrop.Name, eventDrop.EventDropId, eventDrop.Url, eventDrop.EventId, releaseStart, releaseEnd);
    }
    
    public async Task<List<EventDropDdo>> GetEventDrops(int? eventId = null)
    {
        logger.LogTrace("GetEventDrops(eventId={eventId})", eventId);
        
        var events = await db.Events
            .Where(evt => eventId == null || evt.EventId == eventId)
            .Select(evt => new {Event = evt, Drops = db.EventDrops
                .Where(drop => drop.EventId == evt.EventId).ToList() })
            .ToListAsync();

        return events.Select(evt =>
        {
            var evtDdo = ConvertEventEntityToDdo(evt.Event);
            if (!evtDdo.Progressive)
            {
                return evt.Drops.Select(drop => new EventDropDdo(drop.Name, drop.EventDropId, drop.Url, drop.EventId,
                    evtDdo.StartDate, evtDdo.EndDate));
            }

            var ids = evt.Drops.Select(drop => drop.EventDropId).ToList();
            var releaseSlots = EventHelper.GetProgressiveEventDropReleaseSlots(evtDdo.StartDate, evtDdo.Length, ids);
            return evt.Drops.Select(drop =>
            {
                var slot = releaseSlots.First(slot => slot.EventDropId == drop.EventDropId);
                return new EventDropDdo(drop.Name, drop.EventDropId, drop.Url, drop.EventId, slot.Start, slot.End);
            });

        }).SelectMany(evt => evt).ToList();
    }
}