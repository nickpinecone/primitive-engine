using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class CollisionShape : GameObject
{
    private Sprite _debugSprite;

    public Vector2 Size { get; set; }

    public Rectangle WorldRectangle
    {
        get
        {
            return new Rectangle(
                (int)(WorldPosition.X - Size.X * Scale / 2f),
                (int)(WorldPosition.Y - Size.Y * Scale / 2f),
                (int)(Size.X * Scale),
                (int)(Size.Y * Scale)
            );
        }
    }

    public CollisionShape(GameObject parent, Vector2 size) : base(parent)
    {
        Size = size;

        var texture = DebugTexture.GenerateRectTexture((int)Size.X, (int)Size.Y, Color.White);
        var source = new Rectangle(0, 0, (int)Size.X, (int)Size.Y);

        _debugSprite = new Sprite(this, texture, source)
        {
            AccentColor = Color.Blue * 0.3f
        };
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!GameSettings.IsVisibleCollisions) return;

        base.Draw(spriteBatch);
    }
}