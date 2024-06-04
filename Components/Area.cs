using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class Area : GameObject
{
    private Sprite _debugSprite;

    public int Radius { get; set; }
    public bool Hidden { get; set; }

    public Area(GameObject parent, int radius) : base(parent)
    {
        var texture = DebugTexture.GenerateCircleTexture(radius * 2, Color.White);
        var source = new Rectangle(0, 0, radius * 2, radius * 2);

        _debugSprite = new Sprite(this, texture, source)
        {
            AccentColor = Color.LightGray * 0.5f,
            IgnoreScale = true,
        };

        Radius = radius;
    }

    public bool InRadius(GameObject gameObject)
    {
        var distance = Parent.WorldPosition - gameObject.WorldPosition;
        return distance.Length() <= Radius;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Hidden) return;

        base.Draw(spriteBatch);
    }
}