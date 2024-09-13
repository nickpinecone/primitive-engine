using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.Entity;

namespace Primitive.State;

public abstract class BaseState
{
    public event EventHandler<BaseState> OnStateSwitch;

    private List<BaseEntity> _entities = new();
    private List<BaseEntity> _removeQueue = new();

    public void SwitchState(BaseState state)
    {
        OnStateSwitch?.Invoke(this, state);
    }

    public void AddEntity(BaseEntity entity)
    {
        _entities.Add(entity);
    }

    public void RemoveEntity(BaseEntity entity)
    {
        _removeQueue.Add(entity);
    }

    public virtual void Initialize()
    {
        foreach (var entity in _entities)
        {
            entity.Initialize();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var entity in _removeQueue)
        {
            _entities.Remove(entity);
        }
        _removeQueue.Clear();

        foreach (var entity in _entities)
        {
            entity.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var entity in _entities)
        {
            entity.Draw(spriteBatch);
        }
    }
}
