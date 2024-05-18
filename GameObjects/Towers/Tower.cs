using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Tower : GameObject
{
    private Selectable _selectable;
    private WalkPath _walkPath;
    private List<Node> _nodesInRadius;
    private TowerPlot _plot;

    public Sprite Sprite { get { return _selectable.Sprite; } }
    public CollisionShape Shape { get { return _selectable.Shape; } }

    public float DetectRadius { get; set; }

    public Tower(TowerPlot plot, WalkPath walkPath, float detectRadius, Vector2 position, float scale, Texture2D texture, Rectangle source)
    {
        position = plot.WorldPosition - new Vector2(0, source.Height / 5);
        plot.ZIndex = -1;

        _plot = plot;
        _plot.Disabled = true;

        _walkPath = walkPath;
        _selectable = new Selectable(Vector2.Zero, 1f, 2, texture, source, source) { Parent = this };
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

        foreach (var enemy in _walkPath.GetEnemiesToPoints(_nodesInRadius))
        {
            enemy.TakeDamage(1);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _selectable.Draw(spriteBatch);
    }
}