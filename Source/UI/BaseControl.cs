using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.State;
using System.Collections.Generic;

namespace Primitive.UI;

public abstract class BaseControl
{
    public bool Hidden { get; set; } = false;
    public bool Disabled { get; set; } = false;
    public bool Centered { get; set; } = false;
    public int ZIndex { get; set; } = 0;

    public float Rotation { get; set; } = 0f;
    public float Scale { get; set; } = 1f;
    public Vector2 Size { get; set; } = Vector2.Zero;
    public Vector2 Position { get; set; } = Vector2.Zero;

    public Vector2 Origin
    {
        get {
            return Centered ? Size * GlobalScale / 2 : Vector2.Zero;
        }
    }

    public Vector2 Center
    {
        get {
            return Centered ? Position : Position + Size * GlobalScale / 2;
        }
    }

    public Vector2 TopLeft
    {
        get {
            return Centered ? Position - Origin : Position;
        }
    }

    public Rectangle Rect
    {
        get {
            return new Rectangle((int)(TopLeft.X), (int)(TopLeft.Y), (int)(Size.X), (int)(Size.Y));
        }
    }

    public List<BaseControl> Children { get; private set; } = new();
    public BaseControl _parent = null;
    public BaseControl Parent
    {
        get {
            return _parent;
        }
        set {
            _parent?.Children.Remove(this);
            _parent = value;
            _parent?.Children.Add(this);
        }
    }

    public float GlobalRotation
    {
        get {
            return Parent?.Rotation ?? 0f + Rotation;
        }
    }
    public float GlobalScale
    {
        get {
            return Parent?.Scale ?? 1f * Scale;
        }
    }
    public Vector2 GlobalPosition
    {
        get {
            return Parent?.Position ?? Vector2.Zero + Position;
        }
    }

    public BaseControl(BaseState state, BaseControl parent)
    {
        state.AddControl(this);
        Parent = parent;
    }

    public abstract void Initialize();
    public abstract bool HandleInput();
    public abstract void Draw(SpriteBatch spriteBatch);
}
