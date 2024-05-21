using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class Sprite
{
    public GameObject Parent { get; set; }

    public Texture2D Texture { get; protected set; }
    public Color AccentColor { get; set; }

    private Rectangle? _sourceRectangle = null;
    public Rectangle SourceRectangle
    {
        get { return _sourceRectangle ?? Rectangle.Empty; }
        set
        {
            if (_sourceRectangle == null)
            {
                DefaultSource = value;
            }
            _sourceRectangle = value;
        }
    }

    public Rectangle DefaultSource { get; private set; }

    public Vector2 Origin
    {
        get
        {
            return new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);
        }
    }

    public float Scale { get; set; }

    public Sprite(Texture2D texture, Rectangle source)
    {
        Scale = 1f;
        AccentColor = Color.White;
        Texture = texture;
        SourceRectangle = source;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            Parent.WorldPosition,
            SourceRectangle,
            AccentColor,
            Parent.Rotation,
            Origin,
            Parent.Scale * Scale,
            SpriteEffects.None,
            0
        );
    }
}