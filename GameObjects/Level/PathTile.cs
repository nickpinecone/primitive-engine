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

    public PathTile(GameObject parent, Vector2 position, float scale) : base(parent)
    {
        ZIndex = -1;

        var texture = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(615, 515, 160, 105);

        Sprite = new Sprite(this, texture, source);
        Shape = new CollisionShape(this, Sprite.Size);

        AddComponent(Sprite);
        AddComponent(Shape);

        WorldPosition = position;
        Scale = scale;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}