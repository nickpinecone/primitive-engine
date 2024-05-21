using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public enum TowerType { Archer };

abstract class Tower : GameObject
{
    protected WalkPath _walkPath;
    protected List<Node> _nodesInRadius;
    protected TowerPlot _plot;

    public Sprite Sprite { get; set; }
    public CollisionShape Shape { get; set; }
    public Interact Interact { get; set; }

    // TODO Area Component
    public float DetectRadius { get; set; }

    public Tower(GameObject parent, TowerPlot plot, WalkPath walkPath, float detectRadius, Vector2 position, float scale) : base(parent)
    {
        plot.ZIndex = -1;

        _plot = plot;
        _plot.Interact.Disabled = true;

        _walkPath = walkPath;
        _nodesInRadius = _walkPath.GetNodesInRadius(WorldPosition, DetectRadius);

        DetectRadius = detectRadius;
        WorldPosition = position;
        Scale = scale;
    }

    public bool InRadius(Vector2 position)
    {
        return Vector2.Distance(position, WorldPosition) <= DetectRadius;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}