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

            var score1 = await drops.Drops.GetLeagueWeightInTimespan("732281653648687198", DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), DateTimeOffset.Now);
            var count = await drops.Drops.GetLeagueCountInTimespan("732281653648687198", DropHelper.ParseDropTimestamp("2024-02-01 00:00:00"), DateTimeOffset.Now);

            return;
        }
        
    }
}