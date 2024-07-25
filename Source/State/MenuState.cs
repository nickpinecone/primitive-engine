using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Primitive.UI;

namespace Primitive.State;

public class MenuState : BaseState
{
    public override void Initialize(ContentManager content)
    {
        base.Initialize(content);

        var label = new Label("Sample Label");
        label.Position = new Vector2(label.Size.X / 2, label.Size.Y / 2);
        label.Centered = true;
        label.Rotation = MathF.PI / 2;

        AddControl(label);
    }
}
