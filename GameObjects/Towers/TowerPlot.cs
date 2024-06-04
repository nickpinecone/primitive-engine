using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class TowerPlot : GameObject, ISaveable
{
    public event EventHandler<TowerType> OnTowerSelect;

    private ContextMenu _contextMenu;

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public bool Disabled { get; set; }

    public TowerPlot(GameObject parent, Vector2 position, float scale) : base(parent)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(495, 635, 110, 50);

        _contextMenu = new ContextMenu(this, 1f, (source.Height) * scale);

        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        var archerTexture = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var archerSource = new Rectangle(390, 815, 65, 65);
        var archerSprite = new Sprite(null, archerTexture, archerSource);
        _contextMenu.AddItem(archerSprite, TowerType.Archer, Tower.TowerCosts[TowerType.Archer]);
        _contextMenu.OnSelect += HandleSelectTower;

        var fireballTexture = AssetManager.GetAsset<Texture2D>("Towers/Fireball");
        var fireballSource = new Rectangle(0, 0, 41, 94);
        var fireballSprite = new Sprite(null, fireballTexture, fireballSource);
        _contextMenu.AddItem(fireballSprite, TowerType.Magic, Tower.TowerCosts[TowerType.Magic]);

        LocalPosition = position;
        LocalScale = scale;
    }

    private void HandleSelectTower(object sender, object value)
    {
        var towerType = (TowerType)value;

        if (Tower.TowerCosts[towerType] > GameLevelState.Gold) return;

        GameLevelState.Gold -= Tower.TowerCosts[towerType];
        OnTowerSelect?.Invoke(this, towerType);
    }

    public override void HandleInput()
    {
        if (Disabled) return;

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        _contextMenu.Hidden = !Interact.IsSelected;

        base.Update(gameTime);
    }
}