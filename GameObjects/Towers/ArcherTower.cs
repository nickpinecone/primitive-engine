using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class ArcherTower : Tower
{
    public ArcherTower(GameObject parent, TowerPlot plot, WalkPath walkPath, Vector2 position, float scale)
        : base(parent, plot, walkPath, new Damage(20, DamageType.Physical), 300, position, scale)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var source = new Rectangle(65, 180, 160, 185);

        AttackTimer = new Timer(this, 1.5f, false);
        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);
        TowerType = TowerType.Archer;

        LocalPosition -= new Vector2(0, Shape.WorldRectangle.Height / 3f);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (var enemy in _walkPath.GetEnemiesToPoints(_nodesInRadius))
        {
            if (Area.InRadius(enemy) && AttackTimer.Done)
            {
                AttackTimer.Restart();

                var projectile = new ArrowProjectile(null, enemy, Damage, WorldPosition, Scale * 0.5f);

                SpawnObject(projectile);
            }
        }
    }
}