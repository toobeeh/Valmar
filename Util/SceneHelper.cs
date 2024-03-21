namespace Valmar.Util;

public static class SceneHelper
{
    public static int GetScenePrice(int possessedSceneAmount)
    {
        return 20000 * (int)Math.Pow(2, possessedSceneAmount);
    }
}