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

    private List<PathNode> _nextNodes;
    private List<PathNode> _prevNodes;

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public bool FollowMouse { get; set; }
    public Node Node { get; protected set; }
    public int Size { get; protected set; }

    public PathNode(GameObject parent, Node node, NodeType type) : base(parent)
    {
        ZIndex = 1;
        Node = node;
        Size = 20;
        Node.Type = type;

        var texture = DebugTexture.GenerateRectTexture(Size, Size, Color.White);
        var source = new Rectangle(0, 0, Size, Size);

        _nextNodes = new();
        _prevNodes = new();

        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        Interact.OnClick += (_, _) => HandleClick(this, null);
        Interact.OnRightClick += (_, _) => HandleRightClick(this, null);

        AddComponent(Sprite);
        AddComponent(Shape);
        AddComponent(Interact);

        WorldPosition = node.Position;
        Scale = 1f;

        Sprite.AccentColor = type switch
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
            Sprite.AccentColor = Color.Black;
        }

        if (other.Node.Type == NodeType.Regular)
        {
            other.Sprite.AccentColor = Color.Black;
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

        base.HandleInput();

        if (Interact.IsSelected)
        {
            if (Input.IsKeyJustPressed(Keys.D))
            {
                RemoveNode();
                QueueFree();
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
        base.Update(gameTime);

        var mouseState = Mouse.GetState();

        if (FollowMouse)
        {
            WorldPosition = mouseState.Position.ToVector2();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Hidden) return;

        base.Draw(spriteBatch);

        if (SelectedNode == this)
        {
            var mouseState = Mouse.GetState();
            DebugTexture.DrawLineBetween(spriteBatch, SelectedNode.WorldPosition, mouseState.Position.ToVector2(), Size / 5, Sprite.AccentColor);
        }

        foreach (var node in _nextNodes)
        {
            DebugTexture.DrawLineBetween(spriteBatch, WorldPosition, node.WorldPosition, Size / 5, Sprite.AccentColor);
        }
    }
}