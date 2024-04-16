using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Button : GameObject
{
    public event EventHandler OnClick;

    public string Text { get; protected set; }
    public Vector2 TextOrigin { get; protected set; }
    public SpriteFont Font { get; protected set; }

    public Button(Texture2D texture, Rectangle source, string text, SpriteFont font, Vector2 position, float scale)
    {
        Texture = texture;
        Text = text;
        WorldPosition = position;
        SourceRectangle = source;
        Font = font;
        Scale = scale;

        var textSize = font.MeasureString(text);
        TextOrigin = textSize / 2f;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        spriteBatch.DrawString(Font, Text, WorldPosition, Color.White, 0, TextOrigin, Scale, SpriteEffects.None, 0);
    }

    public override void HandleInput()
    {
        var mouseState = Mouse.GetState();

        if (WorldRectangle.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
        {
            OnClick?.Invoke(this, null);
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}