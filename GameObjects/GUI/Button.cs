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

    public Label Label { get; }
    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }

    public Rectangle HoverSource { get; protected set; }

    // Default Button
    public Button(string text, Vector2 position, float scale = 1f)
    {
        var buttonSprite = AssetManager.GetAsset<Texture2D>("GUI/Buttons");
        var sourceRect = new Rectangle(180, 200, 360, 180);
        var hoverRect = new Rectangle(565, 200, 360, 180);

        Label = new Label(Vector2.Zero, 1f, text) { Parent = this };
        Sprite = new(buttonSprite, sourceRect) { Parent = this };
        Shape = new(new Vector2(sourceRect.Width, sourceRect.Height)) { Parent = this };

        WorldPosition = position;
        Scale = scale;
        HoverSource = hoverRect;
    }

    // Custom button
    public Button(Vector2 position, float scale, Texture2D texture, Rectangle source, Rectangle hoverSource)
    {
        Sprite = new(texture, source) { Parent = this };
        Shape = new(new Vector2(source.Width, source.Height)) { Parent = this };
        WorldPosition = position;
        Scale = scale;
        HoverSource = hoverSource;
    }

    // Custom button with text
    public Button(string text, Vector2 position, float scale, Texture2D texture, Rectangle source, Rectangle hoverSource, SpriteFont font)
        : this(position, scale, texture, source, hoverSource)
    {
        Label = new Label(Vector2.Zero, 1f, text, font) { Parent = this };
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch);
        Label?.Draw(spriteBatch);

        if (GameSettings.IsVisibleCollisions)
        {
            Shape.Draw(spriteBatch);
        }
    }

    public override void HandleInput()
    {
        var mouseState = Mouse.GetState();

        if (Shape.WorldRectangle.Contains(mouseState.Position))
        {
            Sprite.SourceRectangle = HoverSource;

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
            Sprite.SourceRectangle = Sprite.DefaultSource;
        }
    }

    public override void Update(GameTime gameTime)
    {
    }
}