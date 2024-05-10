using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class TowerPlot : GameObject
{
    private Selectable _selectable;

    override public float Scale { get { return _selectable.Scale; } }
    override public Rectangle SourceRectangle { get { return _selectable.SourceRectangle; } }

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
        var plot = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var plotSource = new Rectangle(495, 635, 110, 50);

        _selectable = new Selectable(position, scale, 2, plot, plotSource, plotSource);
        _selectable.OnDoubleSelect += HandleSelection;
    }

    public void HandleSelection(object sender, EventArgs args)
    {
        Console.WriteLine("BUIDL TOWERKJll");
    }

    public override void HandleInput()
    {
        _selectable.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        _selectable.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        _selectable.Draw(spriteBatch, graphicsDevice);
    }
}