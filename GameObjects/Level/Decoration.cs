using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

abstract class Decoration : GameObject, ISaveable
{
    public Sprite Sprite { get; }

    public Decoration(GameObject parent, Vector2 position, float scale) : base(parent)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(1165, 465, 115, 170);

        Sprite = new Sprite(this, texture, source);

        LocalPosition = position;
        LocalScale = scale;
    }
}