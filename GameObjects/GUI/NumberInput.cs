
using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class NumberInput : GameObject
{
    private int _value;
    public int Value
    {
        get { return _value; }
        private set
        {
            _value = value;
            Label.Text = value.ToString();
        }
    }

    public Sprite BackSprite { get; }
    public CollisionShape Shape { get; }
    public Label Label { get; }
    public Interact Interact { get; }

    public NumberInput(GameObject parent, Vector2 position, float scale) : base(parent)
    {
        var source = new Rectangle(0, 0, 90, 30);
        var backTexture = DebugTexture.GenerateRectTexture(source.Width, source.Height, Color.Gray);

        BackSprite = new Sprite(this, backTexture, source, 1);
        Shape = new CollisionShape(this, BackSprite.Size);
        Interact = new Interact(this, BackSprite, Shape);
        Label = new Label(this, Vector2.Zero, 1f, "0");

        LocalPosition = position;
        LocalScale = scale;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Interact.IsSelected)
        {
            for (Keys i = Keys.D0; i <= Keys.D9; i++)
            {
                if (Label.Text.Length < 3 && Input.IsKeyJustPressed(i))
                {
                    var num = i - Keys.D0;
                    var numStr = Label.Text + num.ToString();

                    Value = int.Parse(numStr);
                }
            }

            if (Label.Text.Length > 0 && Input.IsKeyJustPressed(Keys.Back))
            {
                var numStr = string.Join("", Label.Text.Take(Label.Text.Length - 1));
                numStr = numStr == "" ? "0" : numStr;
                Value = int.Parse(numStr);
            }
        }
    }
}