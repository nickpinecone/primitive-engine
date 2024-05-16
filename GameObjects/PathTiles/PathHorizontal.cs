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
        SourceRectangle = new Rectangle(615, 515, 160, 105);
    }

    public override void HandleInput()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }
}