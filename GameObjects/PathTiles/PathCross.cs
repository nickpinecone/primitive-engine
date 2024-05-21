using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathCross : PathTile
{
    public PathCross(GameObject parent, Vector2 position, float scale) : base(parent, position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(320, 425, 210, 210);
        Sprite.DefaultSource = Sprite.SourceRectangle;
        Shape.Size = Sprite.Size;
    }
}