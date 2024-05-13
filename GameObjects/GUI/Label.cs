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
    public Vector2 TextSize { get; protected set; }

    public Label(Vector2 position, float scale, string text, SpriteFont font = null)
    {
        font ??= AssetManager.GetAsset<SpriteFont>("GUI/MenuFont");

        WorldPosition = position;
        Scale = scale;

        Text = text;
        Font = font;

        TextSize = Font.MeasureString(Text);
        TextOrigin = TextSize / 2f;
        TextSize *= scale;
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