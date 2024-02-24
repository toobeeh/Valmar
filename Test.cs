using Valmar.Domain.Implementation.Drops;
using Valmar.Util;

namespace Valmar;

public static class Test
{
    public static async Task TestDropChunks(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var drops = scope.ServiceProvider.GetRequiredService<DropCache>();

            var score1 = await drops.Drops.GetLeagueWeight("833571903636504618", DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), DateTimeOffset.Now);
            var count = await drops.Drops.GetLeagueCount("833571903636504618", DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), DateTimeOffset.Now);
            var streak = await drops.Drops.GetLeagueStreak("833571903636504618", DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), DateTimeOffset.Now);
            var time = await drops.Drops.GetLeagueAverageTime("833571903636504618", DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), DateTimeOffset.Now);
            
            var eventd = await drops.Drops.GetEventLeagueDetails(22, "335165475870867456", 55565227);
            var redeemed = DropHelper.FindDropToRedeem(eventd, 4, null);
            return;
        }
        
    }
}