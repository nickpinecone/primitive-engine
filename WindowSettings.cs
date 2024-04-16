using Microsoft.Xna.Framework;

namespace TowerDefense;

public static class WindowSettings
{
    public static readonly int Width = 1280;
    public static readonly int Height = 720;
    public static readonly bool IsFullScreen = false;

    public static Vector2 WindowSize
    {
        get
        {
            return new Vector2(Width, Height);
        }
    }
}