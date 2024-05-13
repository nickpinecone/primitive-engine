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

    public float DetectRadius { get; set; }

    override public float Scale { get { return _selectable.Scale; } }
    override public Rectangle SourceRectangle { get { return _selectable.SourceRectangle; } }

    override public Vector2 WorldPosition
    {
        get { return _selectable.WorldPosition; }
        set
        {
            _selectable.WorldPosition = value;
        }
    }

    public Tower(TowerPlot plot, WalkPath walkPath, float detectRadius, Vector2 position, float scale, Texture2D texture, Rectangle source)
    {
        DetectRadius = detectRadius;

        _plot = plot;
        _plot.Disabled = true;

        _walkPath = walkPath;
        _selectable = new Selectable(position, scale, 2, texture, source, source);
        _nodesInRadius = _walkPath.GetNodesInRadius(WorldPosition, DetectRadius);
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

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        _selectable.Draw(spriteBatch, graphicsDevice);
    }
}