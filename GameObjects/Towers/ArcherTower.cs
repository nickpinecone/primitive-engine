using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;


class ArcherTower : Tower
{
    public ArcherTower(TowerPlot plot, WalkPath walkPath, float detectRadius, Vector2 position, float scale) : base(plot, walkPath, detectRadius, position, scale)
    {
        var sprite = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var source = new Rectangle(65, 180, 160, 185);

        _selectable = new Selectable(Vector2.Zero, 1f, 3, sprite, source, source) { Parent = this };
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (var enemy in _walkPath.GetEnemiesToPoints(_nodesInRadius))
        {
            enemy.TakeDamage(1);
        }
    }
}