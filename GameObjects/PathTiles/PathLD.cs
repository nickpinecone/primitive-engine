using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathLD : PathTile
{
    public PathLD(GameObject parent, Vector2 position, float scale) : base(parent, position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(235, 55, 160, 160);
        Sprite.DefaultSource = Sprite.SourceRectangle;
        Shape.Size = Sprite.Size;
    }
}