using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class LevelPoint : GameObject
{
    public event EventHandler<GameState> OnLevelSelect;

    private Selectable _selectable;
    private GameState _level;

    override public Vector2 WorldPosition { get { return _selectable.WorldPosition; } }
    override public float Scale { get { return _selectable.Scale; } }
    override public Rectangle SourceRectangle { get { return _selectable.SourceRectangle; } }

    public LevelPoint(Vector2 position, float scale, GameState level)
    {
        _level = level;

        var sprite = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(1120, 675, 75, 150);

        _selectable = new Selectable(position, scale, 2, sprite, source, source);
        _selectable.OnDoubleSelect += HandleSelection;
    }

    public void HandleSelection(object sender, EventArgs args)
    {
        OnLevelSelect?.Invoke(this, _level);
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