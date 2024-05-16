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
    private static PathNode SelectedNode = null;
    public static bool Disabled = false;
    public static bool Hidden = false;
    public bool FollowMouse { get; set; }

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

    public PathNode(Node node, NodeType type)
    {
        ZIndex = 2;
        Node = node;
        Size = 20;
        Node.Type = type;

        var texture = DebugTexture.GenerateTexture(Size, Size, Color.White);
        var source = new Rectangle(0, 0, Size, Size);

        _nextNodes = new();
        _prevNodes = new();
        _selectable = new Selectable(node.Position, 1f, 1, texture, source, source);
        _selectable.OnClick += (_, _) => HandleClick(this, null);
        _selectable.OnRightClick += (_, _) => HandleRightClick(this, null);

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
            SelectedNode.LinkNode(nodeSender);
        }
    }

    public void HandleRightClick(object sender, EventArgs args)
    {
        var nodeSender = (PathNode)sender;

        if (SelectedNode != null)
        {
            SelectedNode.UnlinkNode(nodeSender);
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

        SelectedNode = null;
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
        if (PathNode.Disabled) return;

        _selectable.HandleInput();

        if (_selectable.IsSelected)
        {
            if (Input.IsKeyJustPressed(Keys.D))
            {
                RemoveNode();
                OnDelete?.Invoke(this, null);
            }
            if (Input.IsKeyJustPressed(Keys.F))
            {
                FollowMouse = true;
            }
        }

        if (FollowMouse && Input.IsMouseJustPressed(MouseButton.Left))
        {
            FollowMouse = false;
        }

        if (SelectedNode != null && Input.IsKeyJustPressed(Keys.X))
        {
            SelectedNode = null;
        }
    }

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (FollowMouse)
        {
            WorldPosition = mouseState.Position.ToVector2();
        }

        _selectable.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        if (Hidden) return;

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