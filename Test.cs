using System.Diagnostics;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar;

public static class Test
{
    public static async Task TestDropChunks(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var drops = scope.ServiceProvider.GetRequiredService<DropChunkTreeProvider>();
            var tree = drops.GetTree(scope.ServiceProvider);
            while (true)
            {
                /*Console.WriteLine("Enter inspection id");
                var id = Console.ReadLine();

                var tasks = new List<Task>();

                var s = new Stopwatch();
                s.Start();
                var tree = drops.GetTree(scope.ServiceProvider);
                
                tasks.Add(Task.Run(async () =>
                {
                    var score1 = await tree.GetLeagueWeight(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                    Console.WriteLine($"score: {score1}");
                }));
                
                tasks.Add(Task.Run(async () =>
                {
                    var count = await tree.GetLeagueCount(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                    Console.WriteLine($"count: {count}");
                }));
                
                tasks.Add(Task.Run(async () =>
                {
                    var streak = await tree.GetLeagueStreak(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                    Console.WriteLine($"streak: {streak.Streak}/{streak.Head}");
                }));
                
                tasks.Add(Task.Run(async () =>
                {
                    var time = await tree.GetLeagueAverageTime(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                    Console.WriteLine($"time: {time}");
                }));*/
                
                /*var score1 = await drops.Drops.GetLeagueWeight(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                var count = await drops.Drops.GetLeagueCount(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                var streak = await drops.Drops.GetLeagueStreak(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                var time = await drops.Drops.GetLeagueAverageTime(id, DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                
                Console.Write($"score: {score1}, count: {count}, streak: {streak.Streak}/{streak.Head}, time: {time} \n");*/
                /*Task.WaitAll(tasks.ToArray());
                s.Stop();
                Console.WriteLine(s.ElapsedMilliseconds);*/
                
                var s = new Stopwatch();
                s.Start();
                var result = await tree.GetLeagueResults(DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), null);
                var me = result["334048043638849536"];
                s.Stop();
                Console.WriteLine(s.ElapsedMilliseconds);
            }
            
            return;
        }
        
    }
}