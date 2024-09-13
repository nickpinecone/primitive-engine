using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.Entity;

namespace Primitive.State;

public abstract class BaseState
{
    public event EventHandler<BaseState> OnStateSwitch;

    private Dictionary<string, BaseEntity> _entities = new();
    private List<string> _removeQueue = new();

    public void SwitchState(BaseState state)
    {
        OnStateSwitch?.Invoke(this, state);
    }

    public BaseEntity GetEntity(string name)
    {
        return _entities[name];
    }

    public void AddEntity(BaseEntity entity)
    {
        _entities[entity.Name] = entity;
    }

    public void RemoveEntity(BaseEntity entity)
    {
        _removeQueue.Add(entity.Name);
    }

    public virtual void Initialize()
    {
        foreach (var entity in _entities.Values)
        {
            entity.Initialize();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var name in _removeQueue)
        {
            _entities.Remove(name);
        }
        _removeQueue.Clear();

        foreach (var entity in _entities.Values)
        {
            entity.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var entity in _entities.Values)
        {
            entity.Draw(spriteBatch);
        }
    }
}
