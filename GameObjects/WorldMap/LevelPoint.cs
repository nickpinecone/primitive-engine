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

    public LevelPoint(Vector2 position, float scale, GameState level)
    {
        _level = level;

        var flag = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var flagSource = new Rectangle(1120, 675, 75, 150);

        _selectable = new Selectable(position, scale, 2, flag, flagSource, flagSource);
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