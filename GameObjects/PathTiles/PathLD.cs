using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathLD : PathTile
{
    public PathLD(Vector2 position, float scale) : base(position, scale)
    {
        SourceRectangle = new Rectangle(235, 55, 160, 160);
    }
}