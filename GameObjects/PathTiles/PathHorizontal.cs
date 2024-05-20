using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathHorizontal : PathTile
{
    public PathHorizontal(Vector2 position, float scale) : base(position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(615, 515, 160, 105);
    }
}