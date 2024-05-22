using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class Interact : GameObject
{
    public event EventHandler OnClick;
    public event EventHandler OnRightClick;

    public event EventHandler OnSelect;
    public event EventHandler OnDoubleSelect;

    private Sprite _sprite;
    private CollisionShape _shape;

    private bool _isHovered = false;
    public bool IsHovered
    {
        get
        {
            return _isHovered;
        }
        set
        {
            _sprite.IsHovered = value;
            _isHovered = value;
        }
    }

    private bool _isSelected = false;
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _sprite.IsSelected = value;
            _isSelected = value;
        }
    }

    private bool _disabled = false;
    public bool Disabled
    {
        get
        {
            return _disabled;
        }
        set
        {
            _disabled = value;
            if (value)
            {
                IsSelected = false;
                IsHovered = false;
            }
        }
    }

    public Interact(GameObject parent, Sprite sprite, CollisionShape shape) : base(parent)
    {
        _sprite = sprite;
        _shape = shape;
    }

    public override void HandleInput()
    {
        if (Disabled) return;

        base.HandleInput();

        var mouseState = Mouse.GetState();

        if (_shape.WorldRectangle.Contains(mouseState.Position))
        {
            IsHovered = true;

            if (Input.IsMouseJustPressed(MouseButton.Left))
            {
                OnClick?.Invoke(this, null);

                if (!IsSelected)
                {
                    IsSelected = true;
                    OnSelect?.Invoke(this, null);
                }
                else
                {
                    OnDoubleSelect?.Invoke(this, null);
                }
            }

            else if (Input.IsMouseJustPressed(MouseButton.Right))
            {
                OnRightClick?.Invoke(this, null);
            }
        }
        else
        {
            IsHovered = false;

            if (Input.IsMouseJustPressed(MouseButton.Left) || Input.IsMouseJustPressed(MouseButton.Right))
            {
                IsSelected = false;
            }
        }
    }
}