using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public abstract class GameObject
{
    public event EventHandler OnQueueFree;

    public GameObject Parent { get; set; }

    private Vector2 _worldPosition;
    public Vector2 WorldPosition
    {
        get
        {
            return (Parent?.WorldPosition ?? Vector2.Zero) + _worldPosition;
        }
        set
        {
            _worldPosition = value;
        }
    }

    private float _scale;
    public float Scale
    {
        get
        {
            return (Parent?.Scale ?? 1f) * _scale;
        }
        set
        {
            _scale = value;
        }
    }

    private float _rotation;
    public float Rotation
    {
        get
        {
            return (Parent?.Rotation ?? 0) + _rotation;
        }
        set
        {
            _rotation = value;
        }
    }

    public int ZIndex { get; set; }

    public GameObject()
    {
        ZIndex = 0;
    }

    public void QueueFree()
    {
        OnQueueFree?.Invoke(this, null);
    }

    public abstract void HandleInput();
    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}