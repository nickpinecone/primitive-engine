using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Primitive.State;

namespace Primitive.UI;

public class Label : BaseControl
{
    public static SpriteFont Font { get; private set; } = null;

    public static void LoadFont(ContentManager content)
    {
        Font = content.Load<SpriteFont>("UI/Font");
    }

    public Color Color { get; set; } = Color.White;

    private string _text = "";
    public string Text
    {
        get {
            return _text;
        }
        set {
            _text = value;

            if (Font != null)
            {
                Size = Font.MeasureString(value);
            }
        }
    }

    public Label(BaseState state, BaseControl parent, string text, Color? color = null) : base(state, parent)
    {
        Color = color ?? Color;
        Text = text;
    }

    public override void Initialize()
    {
    }

    public override bool HandleInput()
    {
        return false;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Font, Text, Position, Color, Rotation, Origin, Scale, SpriteEffects.None, 0);
    }
}
