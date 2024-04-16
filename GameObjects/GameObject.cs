using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public abstract class GameObject
{
    public Vector2 WorldPosition { get; protected set; }
    public Texture2D Texture { get; protected set; }
    public Rectangle SourceRectangle { get; protected set; }
}