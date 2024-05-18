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

    public Sprite Sprite { get { return _selectable.Sprite; } }
    public CollisionShape Shape { get { return _selectable.Shape; } }

    public LevelPoint(Vector2 position, float scale, GameState level)
    {
        _level = level;

        var sprite = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(1120, 675, 75, 150);

        _selectable = new Selectable(Vector2.Zero, 1f, 2, sprite, source, source) { Parent = this };
        _selectable.OnDoubleSelect += HandleSelection;

        WorldPosition = position;
        Scale = scale;
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

    public override void Draw(SpriteBatch spriteBatch)
    {
        _selectable.Draw(spriteBatch);
    }
}