using Microsoft.Xna.Framework;
using Primitive.Entity;

namespace Primitive.State;

public class GameState : BaseState
{
    public override void Initialize()
    {
        var shape = new ShapeEntity(this, new Vector2(120, 40));
        shape.AttachScript("shape.lua");

        base.Initialize();
    }
}
