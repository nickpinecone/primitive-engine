using System;

namespace TowerDefense;

public static class RandomGenerator
{
    static Random _rng;

    static public Random Rng
    {
        get
        {
            return _rng;
        }
    }

    static RandomGenerator()
    {
        _rng = new Random((int)DateTime.Now.Ticks);
    }
}