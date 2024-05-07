using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerDefense;

class PathNode : GameObject
{
    public event EventHandler OnSelect;

    private static PathNode SelectedNode = null;

    private Selectable _selectable;
    private List<PathNode> _linked;

    public Node Node { get; protected set; }
    public int Size { get; protected set; }

    public PathNode(Node node, int size)
    {
        WorldPosition = node.Position;

        Node = node;
        Size = size;

        var texture = DebugTexture.GenerateTexture(size, size, Color.Black);
        var source = new Rectangle(0, 0, size, size);

        _linked = new();
        _selectable = new Selectable(node.Position, 1f, 1, texture, source, source);
        _selectable.OnClick += (_, _) => HandleClick(this, null);
    }

    public void HandleClick(object sender, EventArgs args)
    {
        var nodeSender = (PathNode)sender;

        if (SelectedNode == null)
        {
            SelectedNode = nodeSender;
        }
        else if (nodeSender != SelectedNode)
        {
            SelectedNode.LinkNode((PathNode)sender);
            SelectedNode = null;
        }

        OnSelect?.Invoke(this, null);
    }

    public void LinkNode(PathNode other)
    {
        Node.LinkNode(other.Node);
        _linked.Add(other);
    }

    public override void HandleInput()
    {
        _selectable.HandleInput();

        if (SelectedNode != null && Input.IsMouseJustPressed(MouseButton.Right))
        {
            SelectedNode = null;
        }
    }

    public override void Update(GameTime gameTime)
    {
        _selectable.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        _selectable.Draw(spriteBatch, graphicsDevice);

        if (SelectedNode == this)
        {
            var mouseState = Mouse.GetState();
            DebugTexture.DrawLineBetween(spriteBatch, SelectedNode.WorldPosition, mouseState.Position.ToVector2(), 3, Color.Black);
        }

        foreach (var node in _linked)
        {
            DebugTexture.DrawLineBetween(spriteBatch, WorldPosition, node.WorldPosition, 3, Color.Black);
        }
    }
}