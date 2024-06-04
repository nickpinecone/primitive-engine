using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class FireballProjectile : Projectile
{
    public FireballProjectile(GameObject parent, Enemy target, Damage damage, Vector2 position, float scale)
        : base(parent, target, damage, 72f, position, scale)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Towers/Fireball");
        var source = new Rectangle(0, 0, 95, 95);

        Sprite = new Sprite(this, texture, source);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        FollowTarget(gameTime);
    }
}