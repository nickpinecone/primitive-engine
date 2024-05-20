using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

abstract class PathTile : GameObject, ISaveable
{
    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }

    public PathTile(Vector2 position, float scale)
    {
        ZIndex = -1;

        var sprite = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle();

        Sprite = new(sprite, source) { Parent = this };
        Shape = new(new Vector2(source.Width, source.Height));

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

        if (GameSettings.IsVisibleCollisions)
        {
            Shape.Draw(spriteBatch);
        }
    }
}