using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class TowerPlot : GameObject
{
    public event EventHandler OnTowerSelect;

    private Selectable _selectable;

    override public Rectangle SourceRectangle { get { return _selectable.SourceRectangle; } }
    override public Texture2D Texture { get { return _selectable.Texture; } }

    public bool Disabled { get; set; }

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

    public TowerPlot(Vector2 position, float scale)
    {
        var sprite = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(495, 635, 110, 50);

        _selectable = new Selectable(position, scale, 2, sprite, source, source);
        _selectable.OnDoubleSelect += HandleSelection;
    }

    public void HandleSelection(object sender, EventArgs args)
    {
        OnTowerSelect?.Invoke(this, null);
    }

    public override void HandleInput()
    {
        if (Disabled) return;

        _selectable.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        if (Disabled) return;

        _selectable.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        _selectable.Draw(spriteBatch, graphicsDevice);
    }
}