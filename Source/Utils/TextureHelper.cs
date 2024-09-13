using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitive.Utils;

public static class TextureHelper
{
    private static GraphicsDeviceManager _graphics = null;

    public static void SetGraphics(GraphicsDeviceManager graphics)
    {
        _graphics = graphics;
    }

    static public Texture2D GenerateRect(Vector2 size, Color color)
    {
        Color[] data = new Color[(int)size.X * (int)size.Y];
        Texture2D texture = new Texture2D(_graphics.GraphicsDevice, (int)size.X, (int)size.Y);

        for (int i = 0; i < data.Length; ++i)
        {
            data[i] = color;
        }

        texture.SetData(data);

        return texture;
    }
}
