using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class CollisionShape : Component
{
    public Vector2 Size { get; set; }

    public Rectangle WorldRectangle
    {
        get
        {
            return new Rectangle(
                (int)(Parent.WorldPosition.X - Size.X * Parent.Scale / 2f),
                (int)(Parent.WorldPosition.Y - Size.Y * Parent.Scale / 2f),
                (int)(Size.X * Parent.Scale),
                (int)(Size.Y * Parent.Scale)
            );
        }
    }

    public CollisionShape(GameObject parent, Vector2 size) : base(parent)
    {
        Size = size;
    }

    public override void HandleInput()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!GameSettings.IsVisibleCollisions) return;

        var rectTexture = DebugTexture.GenerateRectTexture(WorldRectangle.Width, WorldRectangle.Height, Color.White);
        var position = new Vector2(WorldRectangle.Left, WorldRectangle.Top);
        spriteBatch.Draw(rectTexture, position, Color.Blue * 0.5f);
    }
}