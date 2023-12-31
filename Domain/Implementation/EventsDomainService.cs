using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class EventsDomainService(
    ILogger<EventsDomainService> logger, 
    PalantirContext db) : IEventsDomainService
{
    public async Task<EventEntity> GetEventById(int id)
    {
        logger.LogTrace("GetEventById(id={id})", id);
        
        var evt = await db.Events.FirstOrDefaultAsync(evt => evt.EventId == id);
        if (evt is null)
        {
            throw new EntityNotFoundException($"Event with id {id} does not exist.");
        }
        
        return evt;
    }

    public async Task<List<EventEntity>> GetAllEvents()
    {
        return await db.Events.ToListAsync();
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
        return await db.EventDrops.ToListAsync();
    }

    public async Task<List<EventDropEntity>> GetEventDropsOfEvent(int id)
    {
        logger.LogTrace("GetEventDropsOfEvent(id={id})", id);

        var evt = await GetEventById(id); // validate event exists
        return await db.EventDrops.Where(drop => drop.EventId == id).ToListAsync();
    }
}