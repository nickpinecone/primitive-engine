using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public class Placeable : GameObject
{
    static public Stack Actions { get; }
    static public Placeable Selected { get; set; }

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public bool FollowMouse { get; set; }
    public bool DoRotate { get; set; }
    public bool DoScale { get; set; }
    public Type Type { get; protected set; }

    static Placeable()
    {
        var buttonsTexture = AssetManager.GetAsset<Texture2D>("GUI/Buttons", true);
        var trashSource = new Rectangle(1115, 1420, 175, 175);
        var moveSource = new Rectangle(1115, 3420, 175, 175);
        var scaleSource = new Rectangle(1115, 3620, 175, 175);
        var rotateSource = new Rectangle(1115, 3220, 175, 175);

        Actions = new Stack(null, Vector2.Zero, StackDirection.Horizontal);

        var trashButton = new Button(null, Vector2.Zero, 0.4f, buttonsTexture, trashSource, Rectangle.Empty);
        var moveButton = new Button(null, Vector2.Zero, 0.4f, buttonsTexture, moveSource, Rectangle.Empty);
        var scaleButton = new Button(null, Vector2.Zero, 0.4f, buttonsTexture, scaleSource, Rectangle.Empty);
        scaleButton.Sprite.OutlineSize = 2;
        var rotateButton = new Button(null, Vector2.Zero, 0.4f, buttonsTexture, rotateSource, Rectangle.Empty);
        rotateButton.Sprite.OutlineSize = 2;

        Actions.AddItem(trashButton, trashButton.Shape.Size);
        Actions.AddItem(moveButton, moveButton.Shape.Size);
        Actions.AddItem(scaleButton, scaleButton.Shape.Size);
        Actions.AddItem(rotateButton, rotateButton.Shape.Size);

        trashButton.Interact.OnClick += (_, _) =>
        {
            Selected.QueueFree();
            Selected = null;
        };

        moveButton.Interact.OnClick += (_, _) =>
        {
            Selected.FollowMouse = true;
        };

        scaleButton.Interact.OnClick += (_, _) =>
        {
            Selected.Interact.IsSelected = true;
            Selected.DoScale = true;
        };
        scaleButton.Interact.OnDeselect += (_, _) =>
        {
            Selected.DoScale = false;
        };

        rotateButton.Interact.OnClick += (_, _) =>
        {
            Selected.Interact.IsSelected = true;
            Selected.DoRotate = true;
        };
        rotateButton.Interact.OnDeselect += (_, _) =>
        {
            Selected.DoRotate = false;
        };
    }

    public Placeable(GameObject parent, Sprite sprite, Type type, Vector2 position, float scale) : base(parent)
    {
        Sprite = new Sprite(this, sprite.Texture, sprite.SourceRectangle, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        Interact.OnClick += (_, _) => HandleSelect(this, null);

        Type = type;
        LocalPosition = position;
        LocalScale = scale;
    }

    private void HandleSelect(object sender, EventArgs args)
    {
        if (Selected != null && (Selected.DoRotate || Selected.DoScale || Selected.FollowMouse)) return;

        var placeable = (Placeable)sender;
        Selected = placeable;

        Docker.DockToBottomWorld(Actions, Actions.Size, Selected, Selected.Sprite.Size);
    }

    public override void HandleInput()
    {
        if (EditLevelState.EditState != EditState.LevelEditor) return;

        base.HandleInput();

        var keyState = Keyboard.GetState();

        if (Interact.IsSelected)
        {
            if (Input.IsKeyJustPressed(Keys.D))
            {
                QueueFree();
                Selected = null;
            }
            if (Input.IsKeyJustPressed(Keys.F))
            {
                FollowMouse = true;
            }

            if (keyState.IsKeyDown(Keys.LeftShift) || DoScale)
            {
                LocalScale += Input.GetWheelValue() / 1000f;
                LocalScale = Math.Clamp(Scale, 0.1f, 10f);
            }

            if (keyState.IsKeyDown(Keys.LeftControl) || DoRotate)
            {
                LocalRotation -= MathHelper.ToRadians(Input.GetWheelValue() / 120f * 5);
            }
        }

        if (FollowMouse && Input.IsMouseJustPressed(MouseButton.Left))
        {
            FollowMouse = false;
        }

        if (Selected != null)
        {
            Actions.HandleInput();
        }

        if (Selected != null && !Selected.Interact.IsSelected)
        {
            Selected.DoRotate = false;
            Selected.DoScale = false;
            Selected = null;
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var mouseState = Mouse.GetState();

        if (FollowMouse)
        {
            LocalPosition = mouseState.Position.ToVector2();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (Selected != null)
        {
            Actions.Draw(spriteBatch);
        }
    }

    public Placeable Clone()
    {
        var placeable = new Placeable(Parent, Sprite, Type, WorldPosition, Scale);
        placeable.LocalRotation = LocalRotation;
        return placeable;
    }
}