using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Placeable : GameObject
{
    public event EventHandler OnClick;
    public event EventHandler OnDelete;
    public event EventHandler OnMove;

    private Selectable _selectable;

    override public Rectangle SourceRectangle { get { return _selectable.SourceRectangle; } }
    override public Texture2D Texture { get { return _selectable.Texture; } }

    public Type Type { get; protected set; }

    override public float Scale
    {
        get { return _selectable.Scale; }
        set { _selectable.Scale = value; }
    }

    override public Vector2 WorldPosition
    {
        get { return _selectable.WorldPosition; }
        set
        {
            _selectable.WorldPosition = value;
        }
    }

    public Placeable(Texture2D texture, Rectangle source, Type type, Vector2 position, float scale)
    {
        _selectable = new Selectable(position, scale, 2, texture, source, source);
        _selectable.OnClick += HandleClick;
        Type = type;
    }

    public Placeable(GameObject gameObject, Vector2 position, float scale)
    {
        _selectable = new Selectable(position, scale, 2, gameObject.Texture, gameObject.SourceRectangle, gameObject.SourceRectangle);
        _selectable.OnClick += HandleClick;
        Type = gameObject.GetType();
    }

    private void HandleClick(object sender, EventArgs args)
    {
        OnClick?.Invoke(this, null);
    }

    public override void HandleInput()
    {
        _selectable.HandleInput();

        if (_selectable.IsSelected)
        {
            if (Input.IsKeyJustPressed(Keys.D))
            {
                OnDelete?.Invoke(this, null);
            }
            if (Input.IsKeyJustPressed(Keys.F))
            {
                OnMove?.Invoke(this, null);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        _selectable.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        _selectable.Draw(spriteBatch, graphicsDevice);
    }

    public Placeable Clone()
    {
        return new Placeable(this.Texture, this.SourceRectangle, this.Type, this.WorldPosition, this.Scale);
    }
}