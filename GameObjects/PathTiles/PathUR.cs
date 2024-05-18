using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

[Saveable]
class PathUR : PathTile
{
    public PathUR(Vector2 position, float scale) : base(position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(55, 235, 160, 160);
    }
}