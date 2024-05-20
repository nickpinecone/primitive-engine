using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

abstract class Decoration : GameObject, ISaveable
{
    public Sprite Sprite { get; }

    public Decoration(Vector2 position, float scale)
    {
        var sprite = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle();

        Sprite = new(sprite, source) { Parent = this };

        WorldPosition = position;
        Scale = scale;
    }

    public override void HandleInput()
    {
    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch);
    }
}