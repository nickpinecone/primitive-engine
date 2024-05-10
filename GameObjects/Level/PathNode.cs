using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerDefense;

enum PathChangeMode { Link, Delete, Shift };

class PathNode : GameObject
{
    public static PathChangeMode ChangeMode = PathChangeMode.Link;
    private static PathNode SelectedNode = null;

    public event EventHandler OnDelete;

    private Selectable _selectable;
    private List<PathNode> _nextNodes;
    private List<PathNode> _prevNodes;

    public Node Node { get; protected set; }
    public int Size { get; protected set; }

    override public float Scale { get { return _selectable.Scale; } }
    override public Rectangle SourceRectangle { get { return _selectable.SourceRectangle; } }

    override public Vector2 WorldPosition
    {
        get { return _selectable.WorldPosition; }
        set
        {
            Node.Position = value;
            _selectable.WorldPosition = value;
        }
    }

    new public Color AccentColor
    {
        get { return _selectable.AccentColor; }
        set { _selectable.AccentColor = value; }
    }

    public PathNode(Node node, int size, NodeType type)
    {
        Node = node;
        Size = size;
        Node.Type = type;

        var texture = DebugTexture.GenerateTexture(size, size, Color.White);
        var source = new Rectangle(0, 0, size, size);

        _nextNodes = new();
        _prevNodes = new();
        _selectable = new Selectable(node.Position, 1f, 1, texture, source, source);
        _selectable.OnClick += (_, _) => HandleClick(this, null);

        AccentColor = type switch
        {
            NodeType.Start => Color.Green,
            NodeType.End => Color.Blue,
            NodeType.Regular => Color.Red,
            _ => Color.Black,
        };
    }

    public void HandleClick(object sender, EventArgs args)
    {
        var nodeSender = (PathNode)sender;
        if (SelectedNode == nodeSender) return;

        if (SelectedNode == null)
        {
            SelectedNode = nodeSender;
        }
        else
        {
            if (ChangeMode == PathChangeMode.Link)
            {
                SelectedNode.LinkNode(nodeSender);
            }
            else if (ChangeMode == PathChangeMode.Delete)
            {
                SelectedNode.UnlinkNode(nodeSender);
            }
        }
    }

    public void LinkNode(PathNode other)
    {
        if (other.Node.Type == NodeType.Start) return;
        if (this.Node.Type == NodeType.End) return;
        if (other._nextNodes.Contains(this)) return;
        if (_nextNodes.Contains(other)) return;

        this.Node.LinkNode(other.Node);
        LinkPath(other);

        SelectedNode = null;
    }

    public void LinkPath(PathNode other)
    {
        if (this.Node.Type == NodeType.Regular)
        {
            this.AccentColor = Color.Black;
        }

        if (other.Node.Type == NodeType.Regular)
        {
            other.AccentColor = Color.Black;
        }

        _nextNodes.Add(other);
        other._prevNodes.Add(this);
    }

    public void RemoveNode()
    {
        foreach (var node in _nextNodes)
        {
            node._prevNodes.Remove(this);
        }
        _nextNodes = new();
        foreach (var node in _prevNodes)
        {
            node.Node.UnlinkNode(Node);
            node._nextNodes.Remove(this);
        }
    }

    public void UnlinkNode(PathNode other)
    {
        other._prevNodes.Remove(this);
        _nextNodes.Remove(other);
        Node.UnlinkNode(other.Node);

        SelectedNode = null;
    }

    public override void HandleInput()
    {
        var mouseState = Mouse.GetState();
        var keyState = Keyboard.GetState();

        _selectable.HandleInput();

        if (SelectedNode != null && Input.IsMouseJustPressed(MouseButton.Right))
        {
            SelectedNode = null;
        }
        if (ChangeMode == PathChangeMode.Delete)
        {
            if (WorldRectangle.Contains(mouseState.Position) && Input.IsMouseJustPressed(MouseButton.Right))
            {
                RemoveNode();
                OnDelete?.Invoke(this, null);
            }
        }
        else if (ChangeMode == PathChangeMode.Shift)
        {
            if (SelectedNode != null)
            {
                SelectedNode.WorldPosition = mouseState.Position.ToVector2();
            }
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
            DebugTexture.DrawLineBetween(spriteBatch, SelectedNode.WorldPosition, mouseState.Position.ToVector2(), Size / 5, AccentColor);
        }

        foreach (var node in _nextNodes)
        {
            DebugTexture.DrawLineBetween(spriteBatch, WorldPosition, node.WorldPosition, Size / 5, AccentColor);
        }
    }
}