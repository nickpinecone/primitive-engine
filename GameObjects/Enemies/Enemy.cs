using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Enemy : GameObject
{
    public event EventHandler OnDie;
    public event EventHandler OnReachBase;

    private WalkPath _walkPath;

    public int Health { get; set; }

    public float MoveSpeed { get; protected set; }
    public float MovedDistance { get; set; }
    public Node FromNode { get; set; }

    public Enemy(WalkPath walkPath, Node startNode, float moveSpeed, float scale, Texture2D texture, Rectangle source)
    {
        _walkPath = walkPath;
        FromNode = startNode;

        WorldPosition = startNode.Position;
        Scale = scale;
        MoveSpeed = moveSpeed;

        Texture = texture;
        SourceRectangle = source;
    }

    public override void HandleInput()
    {
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            OnDie?.Invoke(this, null);
        }
    }

    public override void Update(GameTime gameTime)
    {
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