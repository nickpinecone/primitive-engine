using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public enum TowerType { Archer };
public enum ActionType { Sell };

abstract class Tower : GameObject
{
    static public Dictionary<TowerType, int> TowerCosts = new() {
        {TowerType.Archer, 100},
    };

    protected WalkPath _walkPath;
    protected List<Node> _nodesInRadius;
    protected TowerPlot _plot;
    protected ContextMenu _contextMenu;

    public Timer AttackTimer { get; set; }
    public Sprite Sprite { get; set; }
    public CollisionShape Shape { get; set; }
    public Interact Interact { get; set; }
    public Area Area { get; set; }
    public Damage Damage { get; set; }
    public TowerType TowerType { get; set; }

    public Tower(GameObject parent, TowerPlot plot, WalkPath walkPath, Damage damage, int detectRadius, Vector2 position, float scale) : base(parent)
    {
        plot.ZIndex = -1;

        Area = new Area(this, detectRadius);

        _plot = plot;
        _plot.Disabled = true;

        _walkPath = walkPath;
        _contextMenu = new ContextMenu(this, 1f, plot.Shape.Size.Y * 1.8f);

        var trashTexture = AssetManager.GetAsset<Texture2D>("GUI/Buttons");
        var trashSource = new Rectangle(1115, 1420, 175, 175);

        _contextMenu.AddItem(new Sprite(null, trashTexture, trashSource), ActionType.Sell);
        _contextMenu.OnSelect += HandleActionSelect;

        Damage = new Damage(damage.Amount, damage.Type);

        LocalPosition = position;
        LocalScale = scale;

        _nodesInRadius = _walkPath.GetNodesInRadius(WorldPosition, detectRadius);
    }

    private void HandleActionSelect(object sender, object action)
    {
        var actionType = (ActionType)action;
        if (actionType == ActionType.Sell)
        {
            GameLevelState.Gold += Tower.TowerCosts[TowerType] / 2;
            _plot.Disabled = false;
            QueueFree();
        }
    }

    public override void Update(GameTime gameTime)
    {
        _contextMenu.Hidden = !Interact.IsSelected;
        Area.Hidden = !Interact.IsSelected;

        base.Update(gameTime);
    }
}