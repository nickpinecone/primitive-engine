using System;
using Microsoft.Xna.Framework;

public class Defense
{
    public float Physical { get; private set; }
    public float Magic { get; private set; }

    public Defense(float physical, float magic)
    {
        Physical = physical;
        Magic = magic;
    }

    public float CalculateDamage(Damage damage)
    {
        float defense = damage.Type switch
        {
            DamageType.Physical => Physical,
            DamageType.Magic => Magic,
            _ => 0,
        };

        // TODO Dummy calc, do better
        float result = damage.Amount - defense;

        return MathHelper.Clamp(result, 1, damage.Amount);
    }
}