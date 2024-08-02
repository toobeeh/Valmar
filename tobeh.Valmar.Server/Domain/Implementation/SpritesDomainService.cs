using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class SpritesDomainService(
    ILogger<SpritesDomainService> logger,
    IEventsDomainService eventsDomainService,
    PalantirContext db) : ISpritesDomainService
{
    public async Task<List<SpriteRankingDdo>> GetSpriteRanking()
    {
        logger.LogTrace("GetSpriteRanking()");

        var sprites = await db.Sprites.Select(sprite => sprite.Id).ToListAsync();
        var inventories = await db.Members.Select(member => member.Sprites)
            .Where(inv => !string.IsNullOrWhiteSpace(inv)).ToListAsync();
        var parsedInventories = inventories
            .Select(inv => inv
                .Split(",")
                .Select(id => new { Id = Convert.ToInt32(id.Replace(".", "")), Active = id.Contains('.') }).ToList())
            .ToList();

        var spriteRanking = sprites.Select(sprite =>
            {
                var bought = parsedInventories.Count(inv => inv.Any(invSprite => invSprite.Id == sprite));
                var active = parsedInventories.Count(inv =>
                    inv.Any(invSprite => invSprite.Id == sprite && invSprite.Active));
                return new SpriteRankingDdo(sprite, active, bought, active * 10 + bought);
            }
        ).ToList();

        // map score property of record to index of ranking
        var orderedRanking = spriteRanking
            .OrderByDescending(sprite => sprite.Rank)
            .Select((sprite, index) => sprite with { Rank = index + 1 })
            .ToList();

        return orderedRanking;
    }

    public async Task<SpriteDdo> GetSpriteById(int id)
    {
        logger.LogTrace("GetSpriteById(id={id})", id);

        var sprite = await db.Sprites.FirstOrDefaultAsync(sprite => sprite.Id == id);
        if (sprite is null)
        {
            throw new EntityNotFoundException($"Sprite with id {id} does not exist.");
        }

        // check if sprite is released, if part of an progressive event
        var released = true;
        if (sprite.EventDropId > 0)
        {
            // find progressive event of eventsprite
            var evt = await db.Events.FirstOrDefaultAsync(evt =>
                evt.Progressive == 1 && db.EventDrops.Any(drop =>
                    drop.EventId == evt.EventId && drop.EventDropId == sprite.EventDropId));

            // check if sprite is released in progressive event
            if (evt != null)
            {
                var eventStart = EventHelper.ParseEventTimestamp(evt.ValidFrom);

                // sprite is released if event passed
                if (DateTimeOffset.UtcNow < eventStart.AddDays(evt.DayLength))
                {
                    var eventDrops = await eventsDomainService.GetEventDrops(evt.EventId);
                    var releaseSlots = EventHelper.GetProgressiveEventDropReleaseSlots(eventStart, evt.DayLength,
                        eventDrops.Select(drop => drop.Id).ToList());

                    // check if drop has been released yet
                    released = releaseSlots.Any(slot => slot.EventDropId == sprite.EventDropId && slot.IsReleased);
                }
            }
        }

        var mappedFlags = FlagHelper.GetFlags(sprite.RequiredFlags ?? 0);

        return new SpriteDdo(sprite.Id, sprite.Name, sprite.Url, sprite.Cost, sprite.Special, sprite.EventDropId,
            sprite.Artist, sprite.Rainbow, released, mappedFlags);
    }

    public async Task<List<SpriteDdo>> GetAllSprites(int? eventId = null)
    {
        logger.LogTrace("GetAllSprites()"); // TODO split to separate geteventsprites.. method

        var eventDrops = await db.EventDrops
            .GroupJoin(
                db.Events.Where(evt => eventId == null || evt.EventId == eventId),
                evt => evt.EventId,
                drop => drop.EventId,
                (drop, events) => new { Drop = drop, Events = events })
            .SelectMany(
                group => group.Events.DefaultIfEmpty(),
                (group, evt) => new { Event = evt, DropId = group.Drop.EventDropId })
            .Where(drop => drop.Event != null)
            .GroupBy(drop => drop.Event)
            .Select(group => new { Event = group.Key!, Drops = group.Select(drop => drop.DropId) })
            .ToListAsync();

        // find all progressive event drops of eventsprites
        var progressiveEventDrops = eventDrops.Where(drop => drop.Event.Progressive == 1).ToList();

        // filter drops of progressive events that are not released yet
        var unreleasedEventDrops = progressiveEventDrops
            .SelectMany(evt =>
            {
                var eventStart = EventHelper.ParseEventTimestamp(evt.Event!.ValidFrom);
                if (DateTimeOffset.UtcNow > eventStart.AddDays(evt.Event.DayLength)) return [];

                return EventHelper.GetProgressiveEventDropReleaseSlots(
                        EventHelper.ParseEventTimestamp(evt.Event.ValidFrom),
                        evt.Event.DayLength, evt.Drops.ToList())
                    .Where(slot => !slot.IsReleased)
                    .Select(slot => slot.EventDropId)
                    .ToList();
            })
            .ToList();

        return (await db.Sprites.ToListAsync())
            .Where(sprite => eventId == null || eventDrops.Any(drop => drop.Drops.Contains(sprite.EventDropId)))
            .Select(sprite =>
            {
                var released =
                    !unreleasedEventDrops.Contains(sprite.EventDropId); // check if drop has been released yet
                if (sprite.Id >= 1000) sprite.EventDropId = 0; // map exclusive sprites out of event
                var mappedFlags = FlagHelper.GetFlags(sprite.RequiredFlags ?? 0);

                return new SpriteDdo(sprite.Id, sprite.Name, sprite.Url, sprite.Cost, sprite.Special,
                    sprite.EventDropId, sprite.Artist, sprite.Rainbow, released, mappedFlags);
            }).ToList();
    }

    public async Task<int> AddSprite(string name, string url, int cost, int? eventDropId,
        string? artist, bool rainbow)
    {
        logger.LogTrace(
            "AddSprite(name={name}, url={url}, cost={cost}, eventDropId={eventDropId}, artist={artist}, rainbow={rainbow})",
            name, url, cost, eventDropId, artist, rainbow);

        if (eventDropId is { } eventDropIdValue)
        {
            await eventsDomainService.GetEventDropById(eventDropIdValue); // check if event drop exists
        }

        var sprite = new SpriteEntity
        {
            Name = name,
            Url = url,
            Cost = cost,
            Special = false,
            EventDropId = eventDropId ?? 0,
            Artist = artist,
            Rainbow = rainbow ? 1 : 0
        };

        var nextId = await db.Sprites.MaxAsync(s => s.Id >= 1000 ? 0 : s.Id) + 1;
        sprite.Id = nextId;
        db.Sprites.Add(sprite);
        await db.SaveChangesAsync();

        return nextId;
    }
}