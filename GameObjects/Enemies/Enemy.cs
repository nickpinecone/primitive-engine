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

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    // TODO Health Component
    public int Health { get; set; }

    public float MoveSpeed { get; protected set; }
    public float MovedDistance { get; set; }
    public Node FromNode { get; set; }

    public Enemy(GameObject parent, WalkPath walkPath, Node startNode, float moveSpeed, int health, float scale, Texture2D texture, Rectangle source) : base(parent)
    {
        _walkPath = walkPath;
        FromNode = startNode;

        WorldPosition = startNode.Position;
        Scale = scale;
        MoveSpeed = moveSpeed;
        Health = health;

        Sprite = new Sprite(this, texture, source);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);
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