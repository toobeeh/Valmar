namespace tobeh.Valmar.Server.Util;

public static class SceneHelper
{
    private const int MaxScenePrice = 250000;

    public static int GetScenePrice(int possessedSceneAmount)
    {
        return Math.Min(MaxScenePrice, 20000 * (int)Math.Pow(2, possessedSceneAmount));
    }
}