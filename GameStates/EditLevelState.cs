using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class EditLevelState : GameState
{
    Label walkPathInfo;
    bool isWalkPathEdit;
    WalkPath walkPath;
    LevelEditor levelEditor;

    public void UpdateWalkPathInfo()
    {
        var text = "Walk Path Edit: " + (isWalkPathEdit ? "On" : "Off");
        var editMode = PathNode.ChangeMode switch
        {
            PathChangeMode.Link => "Link",
            PathChangeMode.Delete => "Delete",
            PathChangeMode.Shift => "Shift",
            _ => "None"
        };
        text += "\nEdit Mode: " + editMode;
        walkPathInfo.Text = text;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Level Editor Setup
        levelEditor = new();
        levelEditor.OnItemPlace += HandleItemPlace;

        AddGameObject(levelEditor);

        // Walk Path Debug Info
        walkPath = new();
        isWalkPathEdit = false;
        walkPathInfo = new Label(Vector2.Zero, 0.5f, "");
        walkPathInfo.AccentColor = Color.Black;
        walkPathInfo.WorldPosition += walkPathInfo.TextSize / 2f;

        UpdateWalkPathInfo();
        AddGameObject(walkPathInfo);
    }

    public override void UnloadContent(ContentManager contentManager)
    {
        AssetManager.UnloadAssets();
    }

    public override void HandleInput()
    {
        var keyState = Keyboard.GetState();
        var mouseState = Mouse.GetState();

        if (Input.IsKeyJustPressed(Keys.Escape))
        {
            SwitchState(new WorldMapState());
        }

        if (Input.IsKeyJustPressed(Keys.W))
        {
            isWalkPathEdit = !isWalkPathEdit;
            PathNode.Disabled = !isWalkPathEdit;
        }

        if (isWalkPathEdit)
        {
            HandleWalkPathInput(mouseState, keyState);
        }

        base.HandleInput();
    }

    public void HandleWalkPathInput(MouseState mouseState, KeyboardState keyState)
    {
        if (Input.IsKeyJustPressed(Keys.A))
        {
            PathNode.ChangeMode = PathChangeMode.Link;
        }
        else if (Input.IsKeyJustPressed(Keys.D))
        {
            PathNode.ChangeMode = PathChangeMode.Delete;
        }
        else if (Input.IsKeyJustPressed(Keys.F))
        {
            PathNode.ChangeMode = PathChangeMode.Shift;
        }

        if (Input.IsMouseJustPressed(MouseButton.Middle))
        {
            var node = new Node(mouseState.Position.ToVector2());
            var nodeType = NodeType.Regular;

            if (keyState.IsKeyDown(Keys.S))
            {
                nodeType = NodeType.Start;
                walkPath.AddStartNode(node);
            }
            else if (keyState.IsKeyDown(Keys.E))
            {
                nodeType = NodeType.End;
            }

            var pathNode = new PathNode(node, nodeType);
            pathNode.OnDelete += HandleNodeDelete;
            AddGameObject(pathNode);
        }

        if (Input.IsKeyJustPressed(Keys.Q))
        {
            walkPath.SaveToFile("walk_path");
        }
        if (Input.IsKeyJustPressed(Keys.R))
        {
            walkPath.LoadFromFile("walk_path");
            GeneratePathNodes();
        }
    }

    public void HandleNodeDelete(object sender, EventArgs args)
    {
        var node = (PathNode)sender;
        RemoveGameObject(node);
    }

    public void GeneratePathNodes()
    {
        Dictionary<Node, PathNode> dict = new();

        foreach (var tuple in walkPath.Enumerate())
        {
            PathNode pathNode;

            if (dict.ContainsKey(tuple.node))
            {
                pathNode = dict[tuple.node];
            }
            else
            {
                pathNode = new PathNode(tuple.node, tuple.node.Type);
                pathNode.OnDelete += HandleNodeDelete;
                dict[tuple.node] = pathNode;
                AddGameObject(pathNode);
            }

            if (tuple.from != null)
            {
                dict[tuple.from].LinkPath(pathNode);
            }
        }
    }

    public void HandleItemPlace(object sender, (Type type, Vector2 position, float scale) data)
    {
        var ctor =
            data.type.GetConstructor(new Type[] { typeof(Vector2), typeof(float) })
            ?? throw new Exception("Game object does not have an appropriate constructor");
        var gameObject = (GameObject)ctor.Invoke(new object[] { data.position, data.scale });

        AddGameObject(gameObject);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateWalkPathInfo();
    }
}