using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Button : GameObject
{
    public Label Label { get; }
    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    // Default Button
    public Button(GameObject parent, string text, Vector2 position, float scale = 1f) : base(parent)
    {
        var texture = AssetManager.GetAsset<Texture2D>("GUI/Buttons");
        var source = new Rectangle(180, 200, 360, 180);
        var hoverSource = new Rectangle(565, 200, 360, 180);

        Label = new Label(this, Vector2.Zero, 1f, text);

        Sprite = new Sprite(this, texture, source, 0, hoverSource);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        AddComponent(Sprite);
        AddComponent(Shape);
        AddComponent(Interact);

        WorldPosition = position;
        Scale = scale;
    }

    // Custom button
    public Button(GameObject parent, Vector2 position, float scale, Texture2D texture, Rectangle source, Rectangle hoverSource) : base(parent)
    {
        Sprite = new Sprite(this, texture, source, 0, hoverSource);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        AddComponent(Sprite);
        AddComponent(Shape);
        AddComponent(Interact);

        WorldPosition = position;
        Scale = scale;
    }

    // Custom button with text
    public Button(GameObject parent, string text, Vector2 position, float scale, Texture2D texture, Rectangle source, Rectangle hoverSource, SpriteFont font)
        : this(parent, position, scale, texture, source, hoverSource)
    {
        Label = new Label(this, Vector2.Zero, 1f, text, font);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        Label?.Draw(spriteBatch);
    }
}