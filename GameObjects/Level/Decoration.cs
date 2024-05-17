using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Decoration : GameObject
{
    public Decoration(Vector2 position, float scale)
    {
        var sprite = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle();

        Texture = sprite;
        SourceRectangle = source;
        WorldPosition = position;
        Scale = scale;
    }

    public override void HandleInput()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }
}