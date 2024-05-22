using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class LevelPoint : GameObject
{
    public event EventHandler<GameState> OnLevelSelect;

    private GameState _level;

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public LevelPoint(GameObject parent, Vector2 position, float scale, GameState level) : base(parent)
    {
        _level = level;

        var texture = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(1120, 675, 75, 150);

        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        Interact.OnDoubleSelect += HandleSelection;

        WorldPosition = position;
        Scale = scale;
    }

    public void HandleSelection(object sender, EventArgs args)
    {
        OnLevelSelect?.Invoke(this, _level);
    }
}