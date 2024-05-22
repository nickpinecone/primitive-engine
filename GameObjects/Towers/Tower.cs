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
    protected WalkPath _walkPath;
    protected List<Node> _nodesInRadius;
    protected TowerPlot _plot;
    protected ContextMenu _contextMenu;

    public Sprite Sprite { get; set; }
    public CollisionShape Shape { get; set; }
    public Interact Interact { get; set; }

    // TODO Area Component
    public float DetectRadius { get; set; }

    public Tower(GameObject parent, TowerPlot plot, WalkPath walkPath, float detectRadius, Vector2 position, float scale) : base(parent)
    {
        plot.ZIndex = -1;

        _plot = plot;
        _plot.Disabled = true;

        _walkPath = walkPath;
        _nodesInRadius = _walkPath.GetNodesInRadius(WorldPosition, DetectRadius);
        _contextMenu = new ContextMenu(this, (plot.Sprite.SourceRectangle.Width + plot.Sprite.SourceRectangle.Height));

        _contextMenu.AddItem(plot.Sprite, ActionType.Sell);
        _contextMenu.OnSelect += HandleActionSelect;

        DetectRadius = detectRadius;
        WorldPosition = position;
        Scale = scale;
    }

    private void HandleActionSelect(object sender, object action)
    {
        var actionType = (ActionType)action;
        if (actionType == ActionType.Sell)
        {
            _plot.Disabled = false;
            QueueFree();
        }
    }

    public bool InRadius(Vector2 position)
    {
        return Vector2.Distance(position, WorldPosition) <= DetectRadius;
    }

    public override void Update(GameTime gameTime)
    {
        _contextMenu.Hidden = !Interact.IsSelected;

        base.Update(gameTime);
    }
}