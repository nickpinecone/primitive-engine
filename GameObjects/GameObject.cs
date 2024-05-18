using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class SaveableAttribute : Attribute { }

public abstract class GameObject
{
    virtual public Vector2 WorldPosition { get; set; }
    virtual public float Scale { get; set; }
    virtual public float Rotation { get; protected set; }
    virtual public Texture2D Texture { get; protected set; }
    public int ZIndex { get; set; }

    public Color AccentColor { get; set; }

    private Rectangle? _sourceRectangle = null;
    virtual public Rectangle SourceRectangle
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

    public GameObject()
    {
        AccentColor = Color.White;
        ZIndex = 0;
    }

    public virtual void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        spriteBatch.Draw(Texture, WorldPosition, SourceRectangle, AccentColor, Rotation, Origin, Scale, SpriteEffects.None, 0);

        if (GameSettings.IsVisibleCollisions)
        {
            DrawCollisionShape(spriteBatch, graphicsDevice);
        }
    }

    private void DrawCollisionShape(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        var rectTexture = DebugTexture.GenerateRectTexture(WorldRectangle.Width, WorldRectangle.Height, Color.White);

        var position = new Vector2(WorldRectangle.Left, WorldRectangle.Top);

        spriteBatch.Draw(rectTexture, position, Color.Blue * 0.5f);
    }

    public abstract void HandleInput();
    public abstract void Update(GameTime gameTime);
}