using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TowerDefense;

class WalkPathEditor : GameObject
{
    public EventHandler<PathNode> OnItemPlace;

    private WalkPath _walkPath;

    private Sprite _selectedItem;
    private NodeType _selectedType;

    // Needs hstack
    private Button _startNode;
    private Button _regularNode;
    private Button _endNode;

    public int SnapAmount { get; set; }

    public WalkPathEditor(GameObject parent, WalkPath walkPath) : base(parent)
    {
        _walkPath = walkPath;

        ZIndex = 1;

        var startNode = new PathNode(null, new Node(Vector2.Zero), NodeType.Start);
        _startNode = new Button(this, Vector2.Zero, 2f, startNode.Sprite.Texture, startNode.Sprite.SourceRectangle, Rectangle.Empty);
        _startNode.Sprite.AccentColor = startNode.Sprite.AccentColor;

        var regularNode = new PathNode(null, new Node(Vector2.Zero), NodeType.Regular);
        _regularNode = new Button(this, Vector2.Zero, 2f, regularNode.Sprite.Texture, regularNode.Sprite.SourceRectangle, Rectangle.Empty);
        _regularNode.Sprite.AccentColor = regularNode.Sprite.AccentColor;

        var endNode = new PathNode(null, new Node(Vector2.Zero), NodeType.End);
        _endNode = new Button(this, Vector2.Zero, 2f, endNode.Sprite.Texture, endNode.Sprite.SourceRectangle, Rectangle.Empty);
        _endNode.Sprite.AccentColor = endNode.Sprite.AccentColor;

        Docker.DockTopRight(_endNode, _endNode.Shape.Size);
        Docker.DockToLeft(_regularNode, _regularNode.Shape.Size, _endNode, _endNode.Shape.Size);
        Docker.DockToLeft(_startNode, _startNode.Shape.Size, _regularNode, _regularNode.Shape.Size);

        _startNode.Interact.OnClick += (_, _) => HandleNodeSelect(_startNode, NodeType.Start);
        _regularNode.Interact.OnClick += (_, _) => HandleNodeSelect(_regularNode, NodeType.Regular);
        _endNode.Interact.OnClick += (_, _) => HandleNodeSelect(_endNode, NodeType.End);
    }

    public void GeneratePathNodes()
    {
        Dictionary<Node, PathNode> dict = new();

        foreach (var tuple in _walkPath.Enumerate())
        {
            PathNode pathNode;

            if (dict.ContainsKey(tuple.node))
            {
                pathNode = dict[tuple.node];
            }
            else
            {
                pathNode = new PathNode(null, tuple.node, tuple.node.Type);
                dict[tuple.node] = pathNode;
                OnItemPlace?.Invoke(this, pathNode);
            }

            if (tuple.from != null)
            {
                dict[tuple.from].LinkPath(pathNode);
            }
        }
    }


    private void HandleNodeSelect(object sender, NodeType type)
    {
        var button = (Button)sender;

        _selectedItem = new Sprite(null, button.Sprite.Texture, button.Sprite.SourceRectangle);
        _selectedItem.AccentColor = button.Sprite.AccentColor;
        _selectedType = type;
    }

    public override void HandleInput()
    {
        if (EditLevelState.EditState != EditState.WalkPath) return;

        var mouseState = Mouse.GetState();
        var keyState = Keyboard.GetState();

        if (_selectedItem != null && Input.IsMouseJustPressed(MouseButton.Left))
        {
            var pathNode = new PathNode(null, new Node(mouseState.Position.ToVector2()), _selectedType);

            OnItemPlace?.Invoke(this, pathNode);
        }

        if (_selectedItem != null && Input.IsKeyJustPressed(Keys.X))
        {
            _selectedItem = null;
        }

        if (Input.IsMouseJustPressed(MouseButton.Middle))
        {
            var node = new Node(mouseState.Position.ToVector2());
            var nodeType = NodeType.Regular;

            if (keyState.IsKeyDown(Keys.S))
            {
                nodeType = NodeType.Start;
            }
            else if (keyState.IsKeyDown(Keys.E))
            {
                nodeType = NodeType.End;
            }

            var pathNode = new PathNode(null, node, nodeType);
            OnItemPlace?.Invoke(this, pathNode);
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        if (EditLevelState.EditState != EditState.WalkPath) return;

        base.Update(gameTime);

        var mouseState = Mouse.GetState();

        if (_selectedItem != null)
        {
            _selectedItem.LocalPosition = mouseState.Position.ToVector2();
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (EditLevelState.EditState != EditState.WalkPath) return;

        base.Draw(spriteBatch);

        _selectedItem?.Draw(spriteBatch);
    }
}