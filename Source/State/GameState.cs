using Microsoft.Xna.Framework;
using Primitive.Entity;

namespace Primitive.State;

public class GameState : BaseState
{
    public override void Initialize()
    {
        var shape =
            new Shape(this, "/home/nick/Development/primitive-engine/Source/Script/shape.lua", new Vector2(120, 40));

        base.Initialize();
    }
}
