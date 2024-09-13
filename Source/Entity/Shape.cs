using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.State;
using Primitive.Utils;

namespace Primitive.Entity;

public class Shape : BaseEntity
{
    public Texture2D Texture { get; private set; } = null;

    public Shape(BaseState state, string file, Vector2 size) : base(state, file)
    {
        Size = size;
    }

    public override void Initialize()
    {
        base.Initialize();

        Texture = TextureHelper.GenerateRectTexture(Size, Color.White);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, null, Color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
    }
}
