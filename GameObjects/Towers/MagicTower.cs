using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class MagicTower : Tower
{
    public MagicTower(GameObject parent, TowerPlot plot, WalkPath walkPath, Vector2 position, float scale)
        : base(parent, plot, walkPath, new Damage(30, DamageType.Magic), 300, position, scale)
    {
        LocalScale *= 0.4f;

        var texture = AssetManager.GetAsset<Texture2D>("Towers/MagicTower");
        var source = new Rectangle(0, 0, 222, 623);

        AttackTimer = new Timer(this, 2f, false);
        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);
        TowerType = TowerType.Archer;

        LocalPosition -= new Vector2(0, Shape.WorldRectangle.Height / 2.4f);

        AddActions();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (var enemy in _walkPath.GetEnemiesToPoints(_nodesInRadius))
        {
            if (Area.InRadius(enemy) && AttackTimer.Done)
            {
                AttackTimer.Restart();

                var projectile = new FireballProjectile(null, enemy, Damage, WorldPosition, Scale * 0.5f);

                SpawnObject(projectile);
            }
        }
    }
}