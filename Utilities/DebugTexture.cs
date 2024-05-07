
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefense;

public static class DebugTexture
{
    static GraphicsDeviceManager graphicsDevice;

    static public void Initialize(GraphicsDeviceManager graphicsDevice)
    {
        DebugTexture.graphicsDevice = graphicsDevice;
    }

    static public Texture2D GenerateTexture(int width, int height, Color color)
    {
        Color[] data = new Color[width * height];
        Texture2D texture = new Texture2D(graphicsDevice.GraphicsDevice, width, height);

        for (int i = 0; i < data.Length; ++i)
        {
            data[i] = color;
        }

        texture.SetData(data);

        return texture;
    }

    private static void DrawLine(SpriteBatch spriteBatch, Vector2 startPos, Vector2 endPos, int thickness, Color color)
    {
        if (startPos == endPos) return;

        var distance = (int)Vector2.Distance(startPos, endPos);
        var texture = new Texture2D(graphicsDevice.GraphicsDevice, distance, thickness);

        var data = new Color[distance * thickness];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = color;
        }
        texture.SetData(data);

        var rotation = (float)Math.Atan2(endPos.Y - startPos.Y, endPos.X - startPos.X);
        var origin = new Vector2(0, thickness / 2);

        spriteBatch.Draw(texture, startPos, null, Color.White, rotation, origin, 1.0f, SpriteEffects.None, 1.0f);
    }

    public static void DrawLineBetween(SpriteBatch spriteBatch, Vector2 startPos, Vector2 endPos, int thickness, Color color)
    {
        DrawLine(spriteBatch, startPos, endPos, thickness, color);

        // Making direction arrow
        var direction = startPos + (endPos - startPos) / 1.1f;
        var arrowSize = thickness * 2;

        DrawLine(spriteBatch, direction + new Vector2(-arrowSize, -arrowSize), direction + new Vector2(arrowSize, arrowSize), thickness, color);
        DrawLine(spriteBatch, direction + new Vector2(arrowSize, -arrowSize), direction + new Vector2(-arrowSize, arrowSize), thickness, color);
    }
}