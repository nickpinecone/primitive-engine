using Microsoft.Xna.Framework;
using Primitive.Entity;

namespace Primitive.State;

public class GameState : BaseState
{
    public override void Initialize()
    {
        var pad = new ShapeEntity(this, "pad", new Vector2(100, 20));
        pad.AttachScript();

        var pong = new ShapeEntity(this, "pong", new Vector2(20, 20));
        pong.AttachScript();

        base.Initialize();
    }
}
