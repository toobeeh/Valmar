namespace Valmar.Util.NChunkTree;

public class DropChunkStore
{
    /*
     *  Drop Store should accomplish the following:
     *  - Get user total drop value (regular and event drops)
     *  - Get user league score/streak/count in time span
     *  - get user event drop value (used/unused)
     *  - consume user eventdrop value 
     *
     *  Testing:
     *
        var drops = new List<PastDropEntity>();
        DropChunkLeaf.drops = drops;
        for (var i = 0; i < 1_000_000; i++)
        {
            PastDropEntity drop = new PastDropEntity
            {
                DropId = i,
                EventDropId = i % 10,
                CaughtLobbyKey = "adasd",
                CaughtLobbyPlayerId = i % 100 + "",
                LeagueWeight = (i % 800) + 200,
                ValidFrom = "adasd"
            };
            drops.Add(drop);
        }
        
        var node = new DropChunkTree(8);

        for (int i = 0; i < drops.Count() / 5000; i++)
        {
            node.AddChunk(new DropChunkLeaf(i * 1000, (i * 1000 + 1)));
        }

        var score = node.GetTotalDropValueForUser("1");
     */
}