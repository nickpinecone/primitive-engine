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
    public Color TextColor { get; set; }

    public Label(GameObject parent, Vector2 position, float scale, string text, SpriteFont font = null) : base(parent)
    {
        ZIndex = 1;

        font ??= AssetManager.GetAsset<SpriteFont>("GUI/MenuFont");

        WorldPosition = position;
        Scale = scale;

        Text = text;
        Font = font;
        TextColor = Color.White;

        TextSize = Font.MeasureString(Text);
        TextOrigin = TextSize / 2f;
        TextSize *= scale;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        spriteBatch.DrawString(
            Font,
            Text,
            WorldPosition,
            TextColor,
            Rotation,
            TextOrigin,
            Scale,
            SpriteEffects.None,
            0
        );
    }
}