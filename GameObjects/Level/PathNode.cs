using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TowerDefense;

class PathNode : GameObject
{
    private Selectable _selectable;

    public Node Node { get; protected set; }
    public int Size { get; protected set; }

    public PathNode(Node node, int size)
    {
        Node = node;
        Size = size;

        var texture = DebugTexture.GenerateTexture(size, size, Color.Black);
        var source = new Rectangle(0, 0, size, size);

        _selectable = new Selectable(node.Position, 1f, 1, texture, source, source);
    }

    public override void HandleInput()
    {
        _selectable.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        _selectable.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        _selectable.Draw(spriteBatch, graphicsDevice);
    }
}