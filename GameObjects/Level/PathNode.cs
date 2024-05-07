using System;
using System.Collections.Generic;
using System.Linq;
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

    private bool _isStart;
    public bool IsStart
    {
        get { return _isStart; }
        set
        {
            _isStart = value;
            AccentColor = value ? Color.Green : Color.Black;
        }
    }

    private bool _isEnd;
    public bool IsEnd
    {
        get { return _isEnd; }
        set
        {
            _isEnd = value;
            AccentColor = value ? Color.Blue : Color.Black;
        }
    }

    override public Vector2 WorldPosition { get { return _selectable.WorldPosition; } }
    override public float Scale { get { return _selectable.Scale; } }
    override public Rectangle SourceRectangle { get { return _selectable.SourceRectangle; } }

    new public Color AccentColor
    {
        get { return _selectable.AccentColor; }
        set { _selectable.AccentColor = value; }
    }

    public PathNode(Node node, int size)
    {
        Node = node;
        Size = size;

        var texture = DebugTexture.GenerateTexture(size, size, Color.White);
        var source = new Rectangle(0, 0, size, size);

        _linked = new();
        _selectable = new Selectable(node.Position, 1f, 1, texture, source, source);
        _selectable.OnClick += (_, _) => HandleClick(this, null);

        AccentColor = Color.Red;
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
            if (nodeSender.IsStart) return;
            if (nodeSender._linked.Contains(SelectedNode)) return;

            if (!SelectedNode.IsStart)
            {
                SelectedNode.AccentColor = Color.Black;
            }

            if (SelectedNode.IsEnd)
            {
                SelectedNode.IsEnd = false;
            }

            nodeSender.AccentColor = Color.Black;

            if (!nodeSender._linked.Any())
            {
                nodeSender.IsEnd = true;
            }

            SelectedNode.LinkNode(nodeSender);
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
            DebugTexture.DrawLineBetween(spriteBatch, SelectedNode.WorldPosition, mouseState.Position.ToVector2(), Size / 5, AccentColor);
        }

        foreach (var node in _linked)
        {
            DebugTexture.DrawLineBetween(spriteBatch, WorldPosition, node.WorldPosition, Size / 5, AccentColor);
        }
    }
}