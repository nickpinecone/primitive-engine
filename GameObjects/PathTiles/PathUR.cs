using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathUR : PathTile
{
    public PathUR(GameObject parent, Vector2 position, float scale) : base(parent, position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(55, 235, 160, 160);
        Sprite.DefaultSource = Sprite.SourceRectangle;
        Shape.Size = Sprite.Size;
    }
}