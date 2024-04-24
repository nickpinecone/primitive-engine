using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Button : GameObject
{
    public event EventHandler OnClick;

    public string Text { get; protected set; }
    public Vector2 TextOrigin { get; protected set; }
    public SpriteFont Font { get; protected set; }
    public Rectangle HoverSource { get; protected set; }

    private Vector2 GetTextOrigin(SpriteFont font, string text)
    {
        var textSize = font.MeasureString(text);
        return textSize / 2f;
    }

    // Default Button
    public Button(string text, Vector2 position, float scale = 1f)
    {
        var buttonSprite = AssetManager.GetAsset<Texture2D>("GUI/Buttons");
        var font = AssetManager.GetAsset<SpriteFont>("GUI/MenuFont");
        var sourceRect = new Rectangle(180, 200, 360, 180);
        var hoverRect = new Rectangle(565, 200, 360, 180);

        Text = text;
        WorldPosition = position;
        Scale = scale;

        Texture = buttonSprite;
        Font = font;
        SourceRectangle = sourceRect;
        HoverSource = hoverRect;
        TextOrigin = GetTextOrigin(font, text);
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
        Text = text;
        Font = font;
        TextOrigin = GetTextOrigin(font, text);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        base.Draw(spriteBatch, graphicsDevice);
        if (Font != null)
            spriteBatch.DrawString(Font, Text, WorldPosition, Color.White, 0, TextOrigin, Scale, SpriteEffects.None, 0);
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