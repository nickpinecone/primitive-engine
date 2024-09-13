using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using Primitive.State;

namespace Primitive.Entity;

public abstract class BaseEntity
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public Vector2 Size { get; set; } = Vector2.Zero;
    public Color Color { get; set; } = Color.White;

    private Lua _state = null;
    private string _file = "";

    public Rectangle Rect
    {
        get
        {
            return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        }
    }

    public BaseEntity(BaseState state, string file)
    {
        state.AddEntity(this);
        _file = file;
    }

    public bool Collide(BaseEntity entity)
    {
        return Rect.Intersects(entity.Rect);
    }

    public virtual void Initialize()
    {
        _state = new Lua();
        _state.LoadCLRPackage();
        //
        _state["position"] = Position;
        // _state["size"] = Size;
        // _state["color"] = Color;
    }

    public virtual void Update(GameTime gameTime)
    {
        _state.DoFile(_file);
        //
        Position = (Vector2)_state["position"];
        // Size = (Vector2)_state["size"];
        // Color = (Color)_state["color"];
    }

    public abstract void Draw(SpriteBatch spriteBatch);
}
