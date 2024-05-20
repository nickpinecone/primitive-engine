using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathVertical : PathTile
{
    public PathVertical(Vector2 position, float scale) : base(position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(430, 235, 105, 160);
        Shape.Size = new Vector2(Sprite.SourceRectangle.Width, Sprite.SourceRectangle.Height);
    }
}