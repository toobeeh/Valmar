using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util;
using tobeh.Valmar.Server.Util.NChunkTree;
using tobeh.Valmar.Server.Util.NChunkTree.Bubbles;

namespace tobeh.Valmar.Server.Domain.Implementation.Bubbles;

/// <summary>
/// Implementation which extends the dropchunkleaf node by the chunk specific functions
/// Main feature is the access and calculation of drop-related data from persistance
/// </summary>
/// <param name="services"></param>
/// <param name="provider"></param>
/// <param name="context"></param>
public class PersistentBubbleChunk : BubbleChunkLeaf, IBubbleChunk
{
    private readonly PalantirContext _db; // guarantee new db instance
    
    public override IBubbleChunk Chunk => this;

    private int? _traceIdStart, _traceIdEnd;

    public PersistentBubbleChunk(IServiceProvider services, BubbleChunkTreeProvider provider, NChunkTreeNodeContext context) : base(services, provider, context)
    {
        _db = ActivatorUtilities.CreateInstance<PalantirContext>(services);
        
        // init chunk with saved range
        Provider.PersistentChunkContext.TryGetValue(NodeId, out var range);
        if (range != null)
        {
            _traceIdStart = range.TraceIdStart;
            _traceIdEnd = range.TraceIdEnd;
            TraceTimestampStart = range.StartDate;
            TraceTimestampEnd = range.EndDate;
        }
    }

    public int? TraceIdStart => _traceIdStart;
    public int? TraceIdEnd => _traceIdEnd;

    public DateTimeOffset? TraceTimestampStart { get; private set; }
    public DateTimeOffset? TraceTimestampEnd { get; private set; }
    
    /// <summary>
    /// Sets the range of the chunk;
    /// Fetches also the dates of the range bounds and stores to the provider for re-fetching
    /// Other instances that are currently alive will be outdated 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void SetChunkSize(int? start, int? end)
    {
        _traceIdStart = start;
        _traceIdEnd = end;

        if (start != null)
        {
            TraceTimestampStart = BubbleHelper.ParseTraceTimestamp(
                _db.BubbleTraces.First(t => t.Id == start).Date);
        }

        if (end != null)
        {
            TraceTimestampEnd = BubbleHelper.ParseTraceTimestamp(
                _db.BubbleTraces.First(t => t.Id == end).Date);
        }

        Provider.PersistentChunkContext[NodeId] = new PersistentBubbleChunkRange(start, end, TraceTimestampStart, TraceTimestampEnd);
    }
    
    public Task<List<int>> EvaluateSubChunks(int chunkSize)
    {
        // find indexes to index chunks
        var traceDateIds = _db.BubbleTraces
            .Where(t => (TraceIdStart == null || t.Id >= TraceIdStart) 
                 && (TraceIdEnd == null || t.Id < TraceIdEnd))
            .GroupBy(trace => trace.Date)
            .Select(group => new
            {
                Date = group.Key, 
                MinId = group.Select(g => g.Id).Min()
            })
            .OrderBy(d => d.MinId)
            .AsEnumerable()
            .Where((trace, index) => (index) % chunkSize == 0)
            .Select(item => item.MinId)
            .ToList();

        return Task.FromResult(traceDateIds);
    }
    
    public async Task<DateTimeOffset?> GetFirstSeenDate(int login)
    {
        var firstTrace = await _db.BubbleTraces.Where(trace =>
                (TraceIdStart == null || trace.Id >= TraceIdStart)
                && (TraceIdEnd == null || trace.Id < TraceIdEnd)
                && trace.Login == login)
            .OrderBy(trace => trace.Id)
            .FirstOrDefaultAsync();
        
        if(firstTrace is null) return null;
        return BubbleHelper.ParseTraceTimestamp(firstTrace.Date);
    }

    public async Task<BubbleTimespanRangeDdo> GetAmountCollectedInTimespan(int login, DateTimeOffset? start, DateTimeOffset? end)
    {
        var traces = await _db.BubbleTraces
            .Where(trace =>
                (TraceIdStart == null || trace.Id >= TraceIdStart)
                && (TraceIdEnd == null || trace.Id < TraceIdEnd)
                && trace.Login == login)
            .ToListAsync();

        var rangeTraces = traces
            .Select(trace => new { Date = BubbleHelper.ParseTraceTimestamp(trace.Date), Bubbles = trace.Bubbles })
            .Where(trace => (start is null || trace.Date >= start) && (end is null || trace.Date < end))
            .OrderBy(trace => trace.Date)
            .ToList();
        
        var min = rangeTraces.Count == 0 ? null : rangeTraces.First();
        var max =  rangeTraces.Count == 0 ? null : rangeTraces.Last();
        var minBubbles = min?.Bubbles;
        var maxBubbles = max?.Bubbles;

        // if chunk and search are open ended or open ended but search end later than last record, check the member's current bubbles
        if (end is null && ChunkEndIndex is null || ChunkEndIndex is null && end is {} endVal && endVal.AddDays(-1) > max?.Date)
            maxBubbles = (await _db.Members.FirstOrDefaultAsync(m => m.Login == login))?.Bubbles ?? maxBubbles;
        
        return new BubbleTimespanRangeDdo(minBubbles, maxBubbles);
    }

    public async Task<List<BubbleProgressEntryDdo>> GetBubbleProgress(int login, DateTimeOffset? start,
        DateTimeOffset? end, BubbleProgressIntervalModeDdo mode)
    {
        var traces = await _db.BubbleTraces
            .Where(trace =>
                (TraceIdStart == null || trace.Id >= TraceIdStart)
                && (TraceIdEnd == null || trace.Id < TraceIdEnd)
                && trace.Login == login)
            .ToListAsync();

        var parsedTraces = traces
            .Select(trace => new BubbleProgressEntryDdo(BubbleHelper.ParseTraceTimestamp(trace.Date),trace.Bubbles))
            .OrderBy(trace => trace.Date)
            .ToList();

        var intervalTraces = parsedTraces.Where(trace =>
        {
            switch (mode)
            {
                case BubbleProgressIntervalModeDdo.Day:
                    return (start is null || trace.Date >= start) &&
                           (end is null || trace.Date < end); // take every trace in timerange
                case BubbleProgressIntervalModeDdo.Week:
                    return trace.Date.DayOfWeek == DayOfWeek.Monday && // take all monday traces
                           (start is null || trace.Date >= start) && (end is null || trace.Date < end);
                case BubbleProgressIntervalModeDdo.Month:
                    return trace.Date.Day == 1 && // take all first of month traces
                           (start is null || trace.Date >= start) && (end is null || trace.Date < end);
                default:
                    return false;
            }
        }).ToList();
        
        // if chunk is open-ended and end is bigger than last trace candidate, add current bubbles
        if (TraceTimestampEnd is null && 
            (end is null || end is { } endVal &&
                (parsedTraces.Count == 0 || endVal.AddDays(-1) > parsedTraces.Last().Date)))
        {
            intervalTraces.Add(new BubbleProgressEntryDdo(DateTimeOffset.Now, 
                (await _db.Members.FirstAsync(m => m.Login == login)).Bubbles));
        }

        return intervalTraces;
    }
}