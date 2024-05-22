using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public abstract class GameObject
{
    public event EventHandler OnQueueFree;

    private List<GameObject> _children;

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

    public GameObject(GameObject parent)
    {
        _children = new();
        _scale = 1f;
        ZIndex = 0;
        Parent = parent;

        Parent?.AddChild(this);
    }

    public void QueueFree()
    {
        OnQueueFree?.Invoke(this, null);
    }

    protected void AddChild(GameObject child)
    {
        _children.Add(child);
    }

    public virtual void HandleInput()
    {
        foreach (var child in _children)
        {
            child.HandleInput();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var child in _children)
        {
            child.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var child in _children)
        {
            child.Draw(spriteBatch);
        }
    }
}