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
    protected Selectable _selectable;
    protected WalkPath _walkPath;
    protected List<Node> _nodesInRadius;
    protected TowerPlot _plot;

    public Sprite Sprite { get { return _selectable.Sprite; } }
    public CollisionShape Shape { get { return _selectable.Shape; } }

    public float DetectRadius { get; set; }

    public Tower(TowerPlot plot, WalkPath walkPath, float detectRadius, Vector2 position, float scale)
    {
        plot.ZIndex = -1;

        _plot = plot;
        _plot.Disabled = true;

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