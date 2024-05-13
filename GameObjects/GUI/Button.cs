using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Button : GameObject
{
    public event EventHandler OnClick;
    public event EventHandler OnRightClick;

    private Label _label;

    public Rectangle HoverSource { get; protected set; }

    // Default Button
    public Button(string text, Vector2 position, float scale = 1f)
    {
        var buttonSprite = AssetManager.GetAsset<Texture2D>("GUI/Buttons");
        var font = AssetManager.GetAsset<SpriteFont>("GUI/MenuFont");
        var sourceRect = new Rectangle(180, 200, 360, 180);
        var hoverRect = new Rectangle(565, 200, 360, 180);

        _label = new Label(position, scale, text, font);

        WorldPosition = position;
        Scale = scale;

        Texture = buttonSprite;
        SourceRectangle = sourceRect;
        HoverSource = hoverRect;
    }

    // Custom button
    public Button(Vector2 position, float scale, Texture2D texture, Rectangle source, Rectangle hoverSource)
    {
        Texture = texture;
        WorldPosition = position;
        SourceRectangle = source;
        Scale = scale;
        HoverSource = hoverSource;
    }

    // Custom button with text
    public Button(string text, Vector2 position, float scale, Texture2D texture, Rectangle source, Rectangle hoverSource, SpriteFont font)
        : this(position, scale, texture, source, hoverSource)
    {
        _label = new Label(position, scale, text, font);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        base.Draw(spriteBatch, graphicsDevice);
        _label?.Draw(spriteBatch, graphicsDevice);
    }

    public override void HandleInput()
    {
        var mouseState = Mouse.GetState();

        if (WorldRectangle.Contains(mouseState.Position))
        {
            SourceRectangle = HoverSource;

            if (Input.IsMouseJustPressed(MouseButton.Left))
            {
                OnClick?.Invoke(this, null);
            }
            else if (Input.IsMouseJustPressed(MouseButton.Right))
            {
                OnRightClick?.Invoke(this, null);
            }
        }
        else
        {
            SourceRectangle = DefaultSource;
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}