using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

abstract class Projectile : GameObject
{
    public Damage Damage { get; }
    public Enemy Target { get; }
    public Sprite Sprite { get; protected set; }

    public float Speed { get; set; }

    protected Projectile(GameObject parent, Enemy target, Damage damage, float speed, Vector2 position, float scale) : base(parent)
    {
        Target = target;
        Speed = speed;
        Damage = damage;

        LocalPosition = position;
        LocalScale = scale;
    }

    protected void FollowTarget(GameTime gameTime)
    {
        var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var direction = Target.WorldPosition - WorldPosition;
        direction.Normalize();

        var velocity = direction * Speed;

        LocalPosition += velocity * delta;

        LocalRotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.ToRadians(135);

        if (Target.Shape.WorldRectangle.Contains(WorldPosition))
        {
            Target.TakeDamage(Damage);
            QueueFree();
        }
    }
}