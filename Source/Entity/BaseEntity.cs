using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.State;

namespace Primitive.Entity;

public abstract class BaseEntity
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Size { get; set; } = Vector2.Zero;
    public Color Color { get; set; } = Color.White;

    private Script _script = null;

    public Rectangle Rect
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }

    public BaseEntity(BaseState state)
    {
        state.AddEntity(this);
    }

    public void AttachScript(string filename)
    {
        var script = new Script(filename);
        _script = script;
    }

    public bool Collide(BaseEntity entity)
    {
        return Rect.Intersects(entity.Rect);
    }

    public virtual void Initialize()
    {
        _script.State["position"] = Position;

        _script.Initialize();
    }

    public virtual void Update(GameTime gameTime)
    {
        _script.Update(gameTime);

        Position = (Vector2)_script.State["position"];
    }

    public abstract void Draw(SpriteBatch spriteBatch);
}
