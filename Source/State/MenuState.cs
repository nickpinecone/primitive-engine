using System;
using Microsoft.Xna.Framework;
using Primitive.UI;

namespace Primitive.State;

public class MenuState : BaseState
{
    public MenuState()
    {
        var label = new Label(this, null, "Sample Label");
        label.Position = new Vector2(label.Size.X / 2, label.Size.Y / 2);
        label.Position += new Vector2(0, 120f);
        label.Centered = true;
        label.Rotation = MathF.PI / 2;
    }
}
