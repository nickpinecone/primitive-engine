using Microsoft.Xna.Framework;

namespace TowerDefense;

static class Docker
{
    static public void DockTopLeft(GameObject gameObject, Vector2 size)
    {
        gameObject.LocalPosition = size * gameObject.Scale / 2f;
    }

    static public void DockTopRight(GameObject gameObject, Vector2 size)
    {
        gameObject.LocalPosition = new Vector2(GameSettings.WindowWidth, 0);
        gameObject.LocalPosition += new Vector2(-size.X, size.Y) * gameObject.Scale / 2f;
    }

    static public void DockBottomLeft(GameObject gameObject, Vector2 size)
    {
        gameObject.LocalPosition = new Vector2(0, GameSettings.WindowHeight);
        gameObject.LocalPosition += new Vector2(size.X, -size.Y) * gameObject.Scale / 2f;
    }

    static public void DockBottomRight(GameObject gameObject, Vector2 size)
    {
        gameObject.LocalPosition = GameSettings.WindowSize;
        gameObject.LocalPosition -= size * gameObject.Scale / 2f;
    }

    static public void DockToRight(GameObject gameObject, Vector2 size, GameObject other, Vector2 otherSize)
    {
        gameObject.LocalPosition = other.WorldPosition + new Vector2(otherSize.X, 0) * other.Scale / 2f;
        gameObject.LocalPosition += new Vector2(size.X, 0) * gameObject.Scale / 2f;
    }

    static public void DockToLeft(GameObject gameObject, Vector2 size, GameObject other, Vector2 otherSize)
    {
        gameObject.LocalPosition = other.WorldPosition - new Vector2(otherSize.X, 0) * other.Scale / 2f;
        gameObject.LocalPosition -= new Vector2(size.X, 0) * gameObject.Scale / 2f;
    }
}