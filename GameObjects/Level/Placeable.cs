using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public class Placeable : GameObject
{
    public static bool Disabled = false;

    public event EventHandler OnClick;
    public event EventHandler OnDelete;

    private Selectable _selectable;

    public Sprite Sprite { get { return _selectable.Sprite; } }
    public CollisionShape Shape { get { return _selectable.Shape; } }

    public bool FollowMouse { get; set; }
    public Type Type { get; protected set; }

    public Placeable(Sprite sprite, Type type, Vector2 position, float scale)
    {
        _selectable = new Selectable(Vector2.Zero, 1f, 2, sprite.Texture, sprite.SourceRectangle, sprite.SourceRectangle) { Parent = this };
        _selectable.OnClick += HandleClick;

        Type = type;
        WorldPosition = position;
        Scale = scale;
    }

    private void HandleClick(object sender, EventArgs args)
    {
        OnClick?.Invoke(this, null);
    }

    public override void HandleInput()
    {
        if (Placeable.Disabled) return;

        _selectable.HandleInput();

        if (_selectable.IsSelected)
        {
            if (Input.IsKeyJustPressed(Keys.D))
            {
                OnDelete?.Invoke(this, null);
            }
            if (Input.IsKeyJustPressed(Keys.F))
            {
                FollowMouse = true;
            }
        }

        if (FollowMouse && Input.IsMouseJustPressed(MouseButton.Left))
        {
            FollowMouse = false;
        }
    }

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        _selectable.Update(gameTime);

        if (FollowMouse)
        {
            WorldPosition = mouseState.Position.ToVector2();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _selectable.Draw(spriteBatch);
    }

    public Placeable Clone()
    {
        return new Placeable(Sprite, Type, WorldPosition, Scale);
    }
}