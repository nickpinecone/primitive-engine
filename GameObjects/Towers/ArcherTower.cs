using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;


class ArcherTower : Tower
{
    public ArcherTower(GameObject parent, TowerPlot plot, WalkPath walkPath, float detectRadius, Vector2 position, float scale) : base(parent, plot, walkPath, detectRadius, position, scale)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var source = new Rectangle(65, 180, 160, 185);

        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        AddComponent(Sprite);
        AddComponent(Shape);
        AddComponent(Interact);
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