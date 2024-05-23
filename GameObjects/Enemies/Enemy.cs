using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

abstract public class Enemy : GameObject
{
    public event EventHandler OnDie;
    public event EventHandler OnReachBase;

    private WalkPath _walkPath;

    public Sprite Sprite { get; protected set; }
    public CollisionShape Shape { get; protected set; }
    public Interact Interact { get; protected set; }

    public Defense Defense { get; protected set; }
    public Health Health { get; }

    public float MoveSpeed { get; protected set; }
    public float MovedDistance { get; set; }
    public Node FromNode { get; set; }

    protected Enemy(GameObject parent, WalkPath walkPath, Node startNode, float moveSpeed, int health, float scale) : base(parent)
    {
        _walkPath = walkPath;
        FromNode = startNode;

        WorldPosition = startNode.Position;
        Scale = scale;
        MoveSpeed = moveSpeed;

        Health = new Health(this, health, scale);
    }

    public void TakeDamage(Damage damage)
    {
        float actual = Defense.CalculateDamage(damage);
        Health.Amount -= actual;

        if (Health.Amount <= 0)
        {
            OnDie?.Invoke(this, null);
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        var toNode = _walkPath.GetNextPoint(this);

        if (toNode == null)
        {
            OnReachBase?.Invoke(this, null);
            return;
        }

        var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var direction = toNode.Position - WorldPosition;
        direction.Normalize();
        var velocity = direction * MoveSpeed * delta;

        WorldPosition += velocity;
        MovedDistance += velocity.Length();
    }
}