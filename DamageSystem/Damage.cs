using System;

public enum DamageType { Physical, Magic };

public class Damage
{
    public DamageType Type { get; private set; }
    public float Amount { get; private set; }

    public Damage(float amount, DamageType type)
    {
        Amount = amount;
        Type = type;
    }
}
