using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class PathTurn : PathTile
{
    public PathTurn(Vector2 position, float scale) : base(position, scale)
    {
        Sprite.SourceRectangle = new Rectangle(70, 410, 210, 160);
        Shape.Size = new Vector2(Sprite.SourceRectangle.Width, Sprite.SourceRectangle.Height);
    }
}