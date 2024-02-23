using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Implementation.Drops;
using Valmar.Util.NChunkTree;

namespace Valmar;

public static class Test
{
    public static async Task TestDropChunks(IServiceProvider services)
    {
        using (var scope = services.CreateScope())
        {
            var drops = scope.ServiceProvider.GetRequiredService<DropCache>();

            var score1 = await drops.Drops.GetTotalLeagueWeight("334048043638849536");

            return;
        }
        
    }
}