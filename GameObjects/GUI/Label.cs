using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Label : GameObject
{
    public string Text { get; set; }
    public Vector2 TextOrigin { get; protected set; }
    public SpriteFont Font { get; protected set; }

    private Vector2 GetTextOrigin(SpriteFont font, string text)
    {
        var textSize = font.MeasureString(text);
        return textSize / 2f;
    }

    public Label(Vector2 position, float scale, string text, SpriteFont font)
    {
        WorldPosition = position;
        Scale = scale;

        Text = text;
        Font = font;
        TextOrigin = GetTextOrigin(font, text);
    }

    public override void HandleInput()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        spriteBatch.DrawString(Font, Text, WorldPosition, AccentColor, Rotation, TextOrigin, Scale, SpriteEffects.None, 0);
    }
}