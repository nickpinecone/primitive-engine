
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class InputForm : GameObject
{
    public Sprite Sprite { get; }
    public Label Label { get; }
    public NumberInput NumberInput { get; }

    public InputForm(GameObject parent, string text, Vector2 position, float scale) : base(parent)
    {
        Label = new Label(this, Vector2.Zero, 1f, text);
        NumberInput = new NumberInput(this, Vector2.Zero, 1f);

        var source = new Rectangle(
            0, 0,
            (int)(Label.Size.X + NumberInput.BackSprite.Size.X) + 20,
            (int)(NumberInput.BackSprite.Size.Y)
        );
        var texture = DebugTexture.GenerateRectTexture(source.Width, source.Height, Color.LightGray);

        Sprite = new Sprite(this, texture, source)
        {
            DrawOrder = -1
        };

        LocalPosition = position;
        LocalScale = scale;
    }

    public override void Update(GameTime gameTime)
    {
        NumberInput.LocalPosition = new Vector2(Sprite.Size.X * Scale / 2f - NumberInput.BackSprite.Size.X * NumberInput.Scale / 2f, 0);
        Label.LocalPosition = new Vector2(-(Sprite.Size.X - 10) * Scale / 2f + Label.Size.X * Scale / 2f, 0);
    }
}