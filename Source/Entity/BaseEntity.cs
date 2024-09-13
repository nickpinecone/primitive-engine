using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Primitive.State;

namespace Primitive.Entity;

public abstract class BaseEntity
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Size { get; set; } = Vector2.Zero;
    public Color Color { get; set; } = Color.White;
    public string Name { get; set; } = null;

    private BaseState _state = null;
    private Script _script = null;

    public Rectangle Rect
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }

    public BaseEntity(BaseState state, string name)
    {
        Name = name;
        _state = state;
        state.AddEntity(this);
    }

    public void AttachScript()
    {
        var script = new Script(Name);
        _script = script;
    }

    public bool Collide(BaseEntity entity)
    {
        return Rect.Intersects(entity.Rect);
    }

    public virtual void Initialize()
    {
        _script.State["position"] = Position;
        _script.State["size"] = Size;
        _script.State["root"] = _state;
        _script.State["this"] = this;

        _script.Initialize();
    }

    public virtual void Update(GameTime gameTime)
    {
        _script.State["keyboard"] = Keyboard.GetState();

        _script.Update(gameTime);

        Position = (Vector2)_script.State["position"];
        Size = (Vector2)_script.State["size"];
    }

    public abstract void Draw(SpriteBatch spriteBatch);
}
