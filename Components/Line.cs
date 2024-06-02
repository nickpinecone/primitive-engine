using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class Line : GameObject
{
    private float _lineRotation;
    private Vector2 _lineOrigin;
    private Texture2D _texture;
    private Sprite _lineDirection;

    public int Thickness { get; set; }

    private Vector2 _startPosition;
    public Vector2 StartPosition
    {
        get { return _startPosition; }
        set
        {
            _startPosition = value;
            Recalculate();
        }
    }

    private Vector2 _endPosition;
    public Vector2 EndPosition
    {
        get { return _endPosition; }
        set
        {
            _endPosition = value;
            Recalculate();
        }
    }
    public Color LineColor { get; set; }

    public Line(GameObject parent, Vector2 startPosition, Vector2 endPosition, Color color, int thickness) : base(parent)
    {
        _startPosition = startPosition;
        _endPosition = endPosition;
        Thickness = thickness;
        LineColor = color;

        Recalculate();
    }

    public void Recalculate(bool primary = true)
    {
        if (StartPosition == EndPosition) return;

        var distance = (int)Vector2.Distance(StartPosition, EndPosition);
        _texture = new Texture2D(DebugTexture.graphicsDevice.GraphicsDevice, distance, Thickness);

        var data = new Color[distance * Thickness];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = LineColor;
        }
        _texture.SetData(data);

        _lineRotation = (float)Math.Atan2(EndPosition.Y - StartPosition.Y, EndPosition.X - StartPosition.X);
        _lineOrigin = new Vector2(0, Thickness / 2);

        var source = new Rectangle(0, 0, 10, 10);
        var texture = DebugTexture.GenerateCircleTexture(source.Width, LineColor);
        _lineDirection = new Sprite(null, texture, source);
        var direction = (EndPosition - StartPosition);
        _lineDirection.LocalPosition = StartPosition + direction / 1.3f;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (_texture != null)
        {
            _lineDirection.Draw(spriteBatch);
            spriteBatch.Draw(_texture, StartPosition, null, Color.White, _lineRotation, _lineOrigin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}