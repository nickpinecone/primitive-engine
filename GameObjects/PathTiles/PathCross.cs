using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathCross : PathTile
{
    public PathCross(Vector2 position, float scale) : base(position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(320, 425, 210, 210);
        Shape.Size = new Vector2(Sprite.SourceRectangle.Width, Sprite.SourceRectangle.Height);
    }
}