using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class ArrowProjectile : Projectile
{
    public ArrowProjectile(GameObject parent, Enemy target, Damage damage, Vector2 position, float scale) : base(parent, target, damage, 36f, position, scale)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var source = new Rectangle(390, 815, 65, 65);

        Sprite = new Sprite(this, texture, source);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        FollowTarget(gameTime);
    }
}