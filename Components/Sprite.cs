using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class Sprite : GameObject
{
    private Texture2D _customTexture;

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

    private Rectangle DefaultSource { get; set; }
    public Rectangle HoverSource { get; set; }

    public Vector2 Origin
    {
        get
        {
            return new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);
        }
    }

    public Vector2 Size
    {
        get
        {
            return new Vector2(SourceRectangle.Width, SourceRectangle.Height);
        }
    }

    public int OutlineSize { get; set; }

    public bool IsHovered { get; set; }
    public bool IsSelected { get; set; }
    public bool Hidden { get; set; }

    public Sprite(GameObject parent, Texture2D texture, Rectangle source, int outlineSize = 0, Rectangle? hoverSource = null) : base(parent)
    {
        _customTexture = DebugTexture.GenerateSpriteTexture(texture, source);

        IsHovered = false;
        IsSelected = false;
        Hidden = false;

        Scale = 1f;
        AccentColor = Color.White;
        Texture = texture;
        SourceRectangle = source;
        HoverSource = hoverSource ?? Rectangle.Empty;
        OutlineSize = outlineSize;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Hidden) return;

        base.Draw(spriteBatch);

        if (IsSelected)
        {
            DrawOutline(spriteBatch);
        }

        spriteBatch.Draw(
            Texture,
            WorldPosition,
            SourceRectangle,
            AccentColor,
            Rotation,
            Origin,
            Scale,
            SpriteEffects.None,
            0
        );

        if (IsHovered)
        {
            if (HoverSource != Rectangle.Empty)
            {
                SourceRectangle = HoverSource;
            }
            else
            {
                DrawHighlight(spriteBatch);
            }
        }
        else if (HoverSource != Rectangle.Empty)
        {
            SourceRectangle = DefaultSource;
        }
    }

    public void DrawHighlight(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _customTexture,
            WorldPosition,
            null,
            Color.White * 0.3f,
            Rotation,
            Origin,
            Scale,
            SpriteEffects.None,
            0
        );
    }

    private void DrawOutline(SpriteBatch spriteBatch)
    {
        if (OutlineSize > 0)
        {
            var positions = new Vector2[4] {
                new Vector2(-OutlineSize, 0),
                new Vector2(OutlineSize, 0),
                new Vector2(0, -OutlineSize),
                new Vector2(0, -OutlineSize)
            };

            foreach (var position in positions)
            {
                spriteBatch.Draw(
                    _customTexture,
                    WorldPosition + position,
                    null,
                    Color.Gray,
                    Rotation,
                    Origin,
                    Scale,
                    SpriteEffects.None,
                    0
                );
            }
        }
    }
}