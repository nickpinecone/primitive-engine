using Microsoft.Xna.Framework;

namespace TowerDefense;

public static class GameSettings
{
    public static readonly bool CreatorMode = false;
    public static readonly bool IsVisibleCollisions = false;

    public static readonly int WindowWidth = 1280;
    public static readonly int WindowHeight = 720;
    public static readonly bool IsFullScreen = false;

    public static Vector2 WindowSize
    {
        get
        {
            return new Vector2(WindowWidth, WindowHeight);
        }
    }
}