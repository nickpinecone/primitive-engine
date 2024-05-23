using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class Health : GameObject
{
    public float MaxAmount { get; private set; }

    public Sprite BackSprite { get; }
    public Sprite Sprite { get; }

    private float _amount;
    public float Amount
    {
        get { return _amount; }
        set
        {
            _amount = value;
            AdjustSprite();
        }
    }

    public Health(GameObject parent, int maxAmount, float scale) : base(parent)
    {
        MaxAmount = maxAmount;
        _amount = maxAmount;

        var backSource = new Rectangle(0, 0, 100, 20);
        var backTexture = DebugTexture.GenerateRectTexture(backSource.Width, backSource.Height, Color.White);
        BackSprite = new Sprite(this, backTexture, backSource)
        {
            AccentColor = Color.SandyBrown
        };

        var texture = DebugTexture.GenerateRectTexture(backSource.Width, backSource.Height, Color.White);
        Sprite = new Sprite(this, texture, backSource)
        {
            AccentColor = Color.Red
        };
    }

    private void AdjustSprite()
    {
        float percent = Amount / MaxAmount;
        int value = (int)(Sprite.SourceRectangle.Width * percent);

        var rect = Sprite.SourceRectangle;
        rect.Width = value;

        Sprite.SourceRectangle = rect;
    }
}