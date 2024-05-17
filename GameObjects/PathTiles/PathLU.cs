
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathLU : PathTile
{
    public PathLU(Vector2 position, float scale) : base(position, scale)
    {
        SourceRectangle = new Rectangle(240, 235, 160, 160);
    }
}