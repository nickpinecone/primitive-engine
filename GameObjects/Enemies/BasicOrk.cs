using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public class BasicOrk : Enemy
{
    public BasicOrk(GameObject parent, WalkPath walkPath, Node startNode, float moveSpeed, int health, float scale) : base(parent, walkPath, startNode, moveSpeed, health, scale)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Enemies/BasicOrk");
        var source = new Rectangle(0, 0, 331, 299);

        Sprite = new Sprite(this, texture, source);
        Shape = new CollisionShape(this, Sprite.Size);

        Health.WorldPosition = new Vector2(-Shape.WorldRectangle.Width / 8f, -Shape.WorldRectangle.Height / 2f);
    }
}