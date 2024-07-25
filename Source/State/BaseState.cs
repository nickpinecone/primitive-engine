using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.Entity;
using Primitive.UI;

namespace Primitive.State;

public abstract class BaseState
{
    public event EventHandler<BaseState> OnStateSwitch;

    private List<BaseControl> _controls = new();
    private List<BaseEntity> _entities = new();
    private List<BaseEntity> _addQueue = new();
    private List<BaseEntity> _removeQueue = new();

    public void SwitchState(BaseState state)
    {
        OnStateSwitch?.Invoke(this, state);
    }

    public void AddControl(BaseControl control)
    {
        _controls.Add(control);
    }

    public void AddEntity(BaseEntity entity)
    {
        _addQueue.Add(entity);
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

        foreach (var control in _controls)
        {
            control.Initialize();
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var entity in _addQueue)
        {
            _entities.Add(entity);
        }
        _addQueue.Clear();

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

    public virtual void HandleInput()
    {
        foreach (var control in _controls.Where((control) => !control.Disabled)
                     .OrderBy((control) => control.ZIndex)
                     .ThenBy((control) => control.Position))
        {
            bool captured = control.HandleInput();

            if (captured)
            {
                break;
            }
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var entity in _entities.OrderByDescending((entity) => entity.Position))
        {
            entity.Draw(spriteBatch);
        }

        foreach (var control in _controls.Where((control) => !control.Hidden)
                     .OrderByDescending((control) => control.ZIndex)
                     .ThenByDescending((control) => control.Position))
        {
            control.Draw(spriteBatch);
        }
    }
}
