
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

    private Button _button;

    public bool IsSelected { get; protected set; }
    public int OutlineSize { get; protected set; }

    public Selectable(Vector2 position, float scale, int outlineSize, Texture2D texture, Rectangle source, Rectangle hoverSource)
    {
        _button = new Button(position, scale, texture, source, hoverSource);
        _button.OnClick += HandleButtonClick;

        OutlineSize = outlineSize;
    }

    public void HandleButtonClick(object sender, EventArgs args)
    {
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

    public override void HandleInput()
    {
        var mouseState = Mouse.GetState();

        _button.HandleInput();

        if (Input.IsMouseJustPressed(MouseButton.Left) && !_button.WorldRectangle.Contains(mouseState.Position))
        {
            IsSelected = false;
        }
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
    }

    private void DrawOutline(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        var outlineTexture = GenerateTexture(graphicsDevice);

        spriteBatch.Draw(outlineTexture, _button.WorldPosition + new Vector2(-OutlineSize, 0), null, Color.Gray, 0, _button.Origin, _button.Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(outlineTexture, _button.WorldPosition + new Vector2(OutlineSize, 0), null, Color.Gray, 0, _button.Origin, _button.Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(outlineTexture, _button.WorldPosition + new Vector2(0, -OutlineSize), null, Color.Gray, 0, _button.Origin, _button.Scale, SpriteEffects.None, 0);
        spriteBatch.Draw(outlineTexture, _button.WorldPosition + new Vector2(0, OutlineSize), null, Color.Gray, 0, _button.Origin, _button.Scale, SpriteEffects.None, 0);
    }

    private Texture2D GenerateTexture(GraphicsDeviceManager graphicsDevice)
    {
        Color[] color = new Color[_button.SourceRectangle.Width * _button.SourceRectangle.Height];
        Texture2D outlineTexture = new Texture2D(graphicsDevice.GraphicsDevice, _button.SourceRectangle.Width, _button.SourceRectangle.Height);
        Color[] buttonColor = new Color[_button.SourceRectangle.Width * _button.SourceRectangle.Height];

        _button.Texture.GetData<Color>(0, _button.SourceRectangle, buttonColor, 0, _button.SourceRectangle.Width * _button.SourceRectangle.Height);

        for (int i = 0; i < color.Length; ++i)
            if (buttonColor[i].A > 0)
            {
                color[i] = Color.White;
            }
            else
                color[i] = Color.Transparent;

        outlineTexture.SetData(color);

        return outlineTexture;
    }
}