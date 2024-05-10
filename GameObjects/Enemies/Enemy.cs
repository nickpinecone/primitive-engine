using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Enemy : GameObject
{
    private WalkPath _walkPath;

    public float MoveSpeed { get; protected set; }
    public float MovedDistance { get; set; }
    public Node FromNode { get; set; }

    public Enemy(WalkPath walkPath, float moveSpeed, Vector2 position, float scale, Texture2D texture, Rectangle source)
    {
        _walkPath = walkPath;
        FromNode = walkPath.GetStartNodes()[0];

        WorldPosition = position;
        Scale = scale;
        MoveSpeed = moveSpeed;

        Texture = texture;
        SourceRectangle = source;
    }

    public override void HandleInput()
    {
    }

    public override void Update(GameTime gameTime)
    {
        var toNode = _walkPath.GetNextPoint(this);

        if (toNode == null) return;

        var direction = toNode.Position - WorldPosition;
        direction.Normalize();
        var velocity = direction * MoveSpeed;

        WorldPosition += velocity;
        MovedDistance += velocity.Length();
    }
}