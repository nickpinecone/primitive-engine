
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
}