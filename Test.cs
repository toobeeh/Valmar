using System.Collections.Concurrent;
using Valmar.Domain.Implementation.Drops;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar;

public static class Test
{
    private record League(double Score, int Count, StreakResult Streak, double Time);
    public static async Task TestDropChunks(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var drops = scope.ServiceProvider.GetRequiredService<DropCache>();
            /*var eventd = await drops.Drops.GetEventLeagueDetails(22, "335165475870867456", 55565227);
            var redeemed = DropHelper.FindDropToRedeem(eventd, 4, null);*/

            /*var pcp = await drops.Drops.GetLeagueParticipants(DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);

            var league = new Dictionary<string, League>();

            var scores = new ConcurrentDictionary<string, double>();
            var counts = new ConcurrentDictionary<string, int>();
            var streaks = new ConcurrentDictionary<string, StreakResult>();
            var times = new ConcurrentDictionary<string, double>();
            
            var tasks = new List<Task>();
            
            var i = 0;
            foreach (var participant in pcp)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var score = await drops.Drops.GetLeagueWeight(participant,
                        DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null); scores.TryAdd(participant, score);
                }));
                
                tasks.Add(Task.Run(async () =>
                {
                    var count = await drops.Drops.GetLeagueCount(participant,
                        DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null); counts.TryAdd(participant, count);
                }));
                
                tasks.Add(Task.Run(async () =>
                {
                    var streak = await drops.Drops.GetLeagueStreak(participant,
                        DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null); streaks.TryAdd(participant, streak);
                }));
                
                tasks.Add(Task.Run(async () =>
                {
                    var time = await drops.Drops.GetLeagueAverageTime(participant,
                        DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null); times.TryAdd(participant, time);
                }));
                Console.WriteLine(++i);
            }

            await Task.WhenAll(tasks);*/
            
            while (true)
            {
                Console.WriteLine("Enter inspection id");
                var id = Console.ReadLine();
                var score1 = await drops.Drops.GetLeagueWeight(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                var count = await drops.Drops.GetLeagueCount(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                var streak = await drops.Drops.GetLeagueStreak(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                var time = await drops.Drops.GetLeagueAverageTime(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                
                Console.Write($"score: {score1}, count: {count}, streak: {streak.Streak}/{streak.Head}, time: {time} \n");
            }
            
            return;
        }
        
    }
}