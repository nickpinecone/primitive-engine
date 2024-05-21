using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class TowerPlot : GameObject, ISaveable
{
    public event EventHandler<TowerType> OnTowerSelect;

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
        _contextMenu = new ContextMenu(80f) { Parent = this };

        var archerTexture = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var archerSource = new Rectangle(390, 815, 65, 65);
        var archerSprite = new Sprite(archerTexture, archerSource);
        _contextMenu.AddItem(archerSprite, TowerType.Archer);
        _contextMenu.OnSelect += HandleSelectTower;

        WorldPosition = position;
        Scale = scale;
    }

    private void HandleSelectTower(object sender, object value)
    {
        var towerType = (TowerType)value;
        OnTowerSelect?.Invoke(this, towerType);
    }

    public override void HandleInput()
    {
        if (Disabled) return;

        _selectable.HandleInput();
        _contextMenu.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
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