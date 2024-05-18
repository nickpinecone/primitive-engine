using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Selectable : GameObject
{
    public event EventHandler OnSelect;
    public event EventHandler OnDoubleSelect;
    public event EventHandler OnClick;
    public event EventHandler OnRightClick;

    private Button _button;
    private Texture2D _customTexture;

    public Sprite Sprite { get { return _button.Sprite; } }
    public CollisionShape Shape { get { return _button.Shape; } }

    public bool IsSelected { get; protected set; }
    public bool IsHovered { get; protected set; }
    public int OutlineSize { get; protected set; }

    public Selectable(Vector2 position, float scale, int outlineSize, Texture2D texture, Rectangle source, Rectangle hoverSource)
    {
        _button = new Button(Vector2.Zero, 1f, texture, source, hoverSource) { Parent = this };
        _button.OnClick += HandleButtonClick;
        _button.OnRightClick += HandleButtonRightClick;

        _customTexture = DebugTexture.GenerateSpriteTexture(Sprite.Texture, source);

        OutlineSize = outlineSize;
        WorldPosition = position;
        Scale = scale;
    }

    private void HandleButtonClick(object sender, EventArgs args)
    {
        OnClick?.Invoke(this, null);

        if (IsSelected)
        {
            OnDoubleSelect?.Invoke(this, null);
        }
        else
        {
            IsSelected = true;
            OnSelect?.Invoke(this, null);
        }
    }

    private void HandleButtonRightClick(object sender, EventArgs args)
    {
        OnRightClick?.Invoke(this, null);
    }

    public override void HandleInput()
    {
        var mouseState = Mouse.GetState();

        _button.HandleInput();

        if (Shape.WorldRectangle.Contains(mouseState.Position))
        {
            IsHovered = true;
        }
        else
        {
            IsHovered = false;
        }

        if (Input.IsMouseJustPressed(MouseButton.Left) && !Shape.WorldRectangle.Contains(mouseState.Position))
        {
            IsSelected = false;
        }
    }

    public void Deselect()
    {
        IsSelected = false;
    }

    public override void Update(GameTime gameTime)
    {
        _button.HandleInput();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (IsSelected)
        {
            DrawOutline(spriteBatch);
        }

        _button.Draw(spriteBatch);

        if (IsHovered)
        {
            DrawHighlight(spriteBatch);
        }
    }

    private void DrawOutline(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_customTexture, WorldPosition + new Vector2(-OutlineSize, 0), null, Color.Gray, 0, Sprite.Origin, Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(_customTexture, WorldPosition + new Vector2(OutlineSize, 0), null, Color.Gray, 0, Sprite.Origin, Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(_customTexture, WorldPosition + new Vector2(0, -OutlineSize), null, Color.Gray, 0, Sprite.Origin, Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(_customTexture, WorldPosition + new Vector2(0, OutlineSize), null, Color.Gray, 0, Sprite.Origin, Scale, SpriteEffects.None, 0);
    }

    private void DrawHighlight(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_customTexture, WorldPosition, null, Color.White * 0.3f, 0, Sprite.Origin, Scale, SpriteEffects.None, 0);
    }
}