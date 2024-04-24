using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public abstract class GameObject
{
    public Vector2 WorldPosition { get; protected set; }
    public Texture2D Texture { get; protected set; }
    public float Scale { get; protected set; }
    public float Rotation { get; protected set; }

    private Rectangle? _sourceRectangle = null;
    public Rectangle SourceRectangle
    {
        get { return _sourceRectangle ?? Rectangle.Empty; }
        set
        {
            if (_sourceRectangle == null)
            {
                DefaultSource = value;
            }
            _sourceRectangle = value;
        }
    }

    public Rectangle DefaultSource { get; private set; }

    public Rectangle WorldRectangle
    {
        get
        {
            return new Rectangle((int)(WorldPosition.X - SourceRectangle.Width * Scale / 2f), (int)(WorldPosition.Y - SourceRectangle.Height * Scale / 2f), (int)(SourceRectangle.Width * Scale), (int)(SourceRectangle.Height * Scale));
        }
    }
    public Vector2 Origin
    {
        get
        {
            return new Vector2(SourceRectangle.Width / 2f, SourceRectangle.Height / 2f);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        spriteBatch.Draw(Texture, WorldPosition, SourceRectangle, Color.White, Rotation, Origin, Scale, SpriteEffects.None, 0);

        if (GameSettings.IsVisibleCollisions)
        {
            DrawCollisionShape(spriteBatch, graphicsDevice);
        }
    }

    private void DrawCollisionShape(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        Color[] color = new Color[Texture.Width * Texture.Height];
        Texture2D rectTexture = new Texture2D(graphicsDevice.GraphicsDevice, Texture.Width, Texture.Height);

        for (int i = 0; i < color.Length; ++i)
            color[i] = Color.White;

        rectTexture.SetData(color);
        var position = new Vector2(WorldRectangle.Left, WorldRectangle.Top);

        spriteBatch.Draw(rectTexture, position, Color.Blue * 0.5f);
    }

    public abstract void HandleInput();
    public abstract void Update(GameTime gameTime);
}