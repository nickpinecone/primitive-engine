using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Tree : PathTile
{
    public Tree(Vector2 position, float scale) : base(position, scale)
    {
        SourceRectangle = new Rectangle(1165, 465, 115, 170);
    }
}