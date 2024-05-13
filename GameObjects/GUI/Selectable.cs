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

    public bool IsSelected { get; protected set; }
    public bool IsHovered { get; protected set; }
    public int OutlineSize { get; protected set; }

    override public float Scale { get { return _button.Scale; } }
    override public Rectangle SourceRectangle { get { return _button.SourceRectangle; } }
    override public Texture2D Texture { get { return _button.Texture; } }

    override public Vector2 WorldPosition
    {
        get { return _button.WorldPosition; }
        set { _button.WorldPosition = value; }
    }

    new public Color AccentColor
    {
        get { return _button.AccentColor; }
        set { _button.AccentColor = value; }
    }

    public Selectable(Vector2 position, float scale, int outlineSize, Texture2D texture, Rectangle source, Rectangle hoverSource)
    {
        _button = new Button(position, scale, texture, source, hoverSource);
        _button.OnClick += HandleButtonClick;
        _button.OnRightClick += HandleButtonRightClick;

        OutlineSize = outlineSize;
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

        if (WorldRectangle.Contains(mouseState.Position))
        {
            IsHovered = true;
        }
        else
        {
            IsHovered = false;
        }

        if (Input.IsMouseJustPressed(MouseButton.Left) && !WorldRectangle.Contains(mouseState.Position))
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

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        // Doesnt draw itself, just a container
        // base.Draw(spriteBatch, graphicsDevice);

        if (IsSelected)
        {
            DrawOutline(spriteBatch, graphicsDevice);
        }

        _button.Draw(spriteBatch, graphicsDevice);

        if (IsHovered)
        {
            DrawHighlight(spriteBatch, graphicsDevice);
        }
    }

    private void DrawOutline(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        var outlineTexture = GenerateTexture(graphicsDevice);

        spriteBatch.Draw(outlineTexture, WorldPosition + new Vector2(-OutlineSize, 0), null, Color.Gray, 0, Origin, Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(outlineTexture, WorldPosition + new Vector2(OutlineSize, 0), null, Color.Gray, 0, Origin, Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(outlineTexture, WorldPosition + new Vector2(0, -OutlineSize), null, Color.Gray, 0, Origin, Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(outlineTexture, WorldPosition + new Vector2(0, OutlineSize), null, Color.Gray, 0, Origin, Scale, SpriteEffects.None, 0);
    }

    private void DrawHighlight(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        var highlightTexture = GenerateTexture(graphicsDevice);

        spriteBatch.Draw(highlightTexture, WorldPosition, null, Color.White * 0.3f, 0, Origin, Scale, SpriteEffects.None, 0);
    }

    private Texture2D GenerateTexture(GraphicsDeviceManager graphicsDevice)
    {
        Color[] color = new Color[SourceRectangle.Width * SourceRectangle.Height];
        Texture2D outlineTexture = new Texture2D(graphicsDevice.GraphicsDevice, SourceRectangle.Width, SourceRectangle.Height);
        Color[] buttonColor = new Color[SourceRectangle.Width * SourceRectangle.Height];

        _button.Texture.GetData<Color>(0, SourceRectangle, buttonColor, 0, SourceRectangle.Width * SourceRectangle.Height);

        for (int i = 0; i < color.Length; ++i)
        {
            if (buttonColor[i].A > 0)
            {
                color[i] = Color.White;
            }
            else
            {
                color[i] = Color.Transparent;
            }
        }

        outlineTexture.SetData(color);

        return outlineTexture;
    }
}