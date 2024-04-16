using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public abstract class GameObject
{
    public Vector2 WorldPosition { get; protected set; }
    public Texture2D Texture { get; protected set; }
    public Rectangle SourceRectangle { get; protected set; }
    public float Scale { get; protected set; }
    public float Rotation { get; protected set; }

    public Rectangle WorldRectangle
    {
        get
        {
            return new Rectangle((int)(WorldPosition.X - SourceRectangle.Width * Scale / 2f), (int)(WorldPosition.Y - SourceRectangle.Height * Scale / 2f), (int)(SourceRectangle.Width * Scale), (int)(SourceRectangle.Y * Scale));
        }
    }
    public Vector2 Origin
    {
        get
        {
            return new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, WorldPosition, SourceRectangle, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);
    }

    public abstract void HandleInput();
    public abstract void Update(GameTime gameTime);
}