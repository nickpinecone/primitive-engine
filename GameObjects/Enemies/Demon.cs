using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public class Demon : Enemy
{
    public Demon(GameObject parent, WalkPath walkPath, Node startNode, float scale)
        : base(parent, walkPath, startNode, 36f, 80, scale)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Enemies/Demon");
        var source = new Rectangle(0, 0, 292, 248);

        Sprite = new Sprite(this, texture, source);
        Shape = new CollisionShape(this, new Vector2(Sprite.Size.X / 2f, Sprite.Size.Y / 1.5f));
        Shape.LocalPosition += new Vector2(-20, 0);
        Defense = new Defense(5, 15);

        HeartsOff = 1;
        KillMoney = 15;

        Health.LocalPosition += new Vector2(-Shape.WorldRectangle.Width / 8f, -Shape.WorldRectangle.Height / 2f);
    }
}