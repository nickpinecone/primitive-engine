using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Label : GameObject
{
    private string _text;
    public string Text
    {
        get { return _text; }
        set
        {
            _text = value;

            TextSize = Font.MeasureString(_text);
            TextOrigin = TextSize / 2f;
            TextSize *= Scale;
        }
    }

    public Vector2 TextOrigin { get; protected set; }
    public SpriteFont Font { get; protected set; }
    public Vector2 TextSize { get; protected set; }
    public Color TextColor { get; set; }

    public Label(GameObject parent, Vector2 position, float scale, string text, SpriteFont font = null) : base(parent)
    {
        ZIndex = 1;

        font ??= AssetManager.GetAsset<SpriteFont>("GUI/MenuFont");

        LocalPosition = position;
        LocalScale = scale;

        Font = font;
        TextColor = Color.White;

        Text = text;
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