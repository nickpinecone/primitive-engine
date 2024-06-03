using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public abstract class GameObject
{
    public event EventHandler OnQueueFree;
    public event EventHandler<GameObject> OnSpawnObject;

    private List<GameObject> _children;

    public GameObject _parent = null;
    public GameObject Parent
    {
        get { return _parent; }
        set
        {
            _parent?.RemoveChild(this);
            _parent = value;
            _parent?.AddChild(this);
        }
    }

    public Vector2 LocalPosition { get; set; }
    public Vector2 WorldPosition
    {
        get
        {
            return (Parent?.WorldPosition ?? Vector2.Zero) + LocalPosition;
        }
    }

    public float LocalScale;
    public float Scale
    {
        get
        {
            return (Parent?.Scale ?? 1f) * LocalScale;
        }
    }

    public float LocalRotation;
    public float Rotation
    {
        get
        {
            return (Parent?.Rotation ?? 0) + LocalRotation;
        }
    }

    public int ZIndex { get; set; }
    public int DrawOrder { get; set; }

    public GameObject(GameObject parent)
    {
        _children = new();

        LocalScale = 1f;
        ZIndex = 0;
        DrawOrder = 0;
        Parent = parent;
    }

    protected virtual void QueueFree()
    {
        OnQueueFree?.Invoke(this, null);
    }

    protected void SpawnObject(GameObject gameObject)
    {
        OnSpawnObject?.Invoke(this, gameObject);
    }

    protected void AddChild(GameObject child)
    {
        _children.Add(child);
    }

    protected void RemoveChild(GameObject child)
    {
        _children.Remove(child);
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
        foreach (var child in _children.OrderBy((child) => child.DrawOrder))
        {
            child.Draw(spriteBatch);
        }
    }
}