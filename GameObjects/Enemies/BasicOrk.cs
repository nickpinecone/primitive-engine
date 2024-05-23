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
        Shape = new CollisionShape(this, new Vector2(Sprite.Size.X / 2f, Sprite.Size.Y / 1.5f));
        Shape.LocalPosition += new Vector2(-20, 0);
        Defense = new Defense(0, 0);

        Health.LocalPosition += new Vector2(-Shape.WorldRectangle.Width / 8f, -Shape.WorldRectangle.Height / 2f);
    }
}