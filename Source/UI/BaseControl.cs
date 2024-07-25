using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitive.UI;

public abstract class BaseControl
{
    public Vector2 Position { get; set; } = Vector2.Zero;
    public int ZIndex { get; set; } = 0;

    public abstract void Initialize();
    public abstract bool HandleInput();
    public abstract void Draw(SpriteBatch spriteBatch);
}
