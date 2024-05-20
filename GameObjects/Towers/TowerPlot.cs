using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class TowerPlot : GameObject, ISaveable
{
    public event EventHandler OnTowerSelect;

    private Selectable _selectable;
    private ContextMenu _contextMenu;

    public Sprite Sprite { get { return _selectable.Sprite; } }
    public CollisionShape Shape { get { return _selectable.Shape; } }

    public bool Disabled { get; set; }

    public TowerPlot(Vector2 position, float scale)
    {
        var sprite = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(495, 635, 110, 50);

        _selectable = new Selectable(Vector2.Zero, 1f, 2, sprite, source, source) { Parent = this };
        _contextMenu = new ContextMenu(50f) { Parent = this };

        _contextMenu.AddItem(Sprite, 1);

        WorldPosition = position;
        Scale = scale;
    }

    public override void HandleInput()
    {
        if (Disabled) return;

        _selectable.HandleInput();
        _contextMenu.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        if (Disabled) return;

        _contextMenu.Hidden = !_selectable.IsSelected;

        _selectable.Update(gameTime);
        _contextMenu.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _selectable.Draw(spriteBatch);
        _contextMenu.Draw(spriteBatch);
    }
}