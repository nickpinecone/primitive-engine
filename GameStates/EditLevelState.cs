using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class EditLevelState : GameState
{
    Label editInfo;
    bool isWalkPathEdit;

    WalkPath walkPath;
    LevelEditor levelEditor;

    public void UpdateWalkPathInfo()
    {
        var text = "Edit Info: " + (isWalkPathEdit ? "Walk Path" : "Level Editor");
        editInfo.Text = text;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        // Level Editor Setup
        levelEditor = new();
        levelEditor.OnOverlay += HandleOverlay;

        AddGameObject(levelEditor);

        // Walk Path Debug Info
        walkPath = new();
        isWalkPathEdit = false;
        editInfo = new Label(Vector2.Zero, 0.5f, "");
        editInfo.AccentColor = Color.Black;
        editInfo.WorldPosition += editInfo.TextSize / 2f;

        UpdateWalkPathInfo();
        AddGameObject(editInfo);
    }

    private void HandleOverlay(object sender, bool isOpen)
    {
        PathNode.Hidden = isOpen;
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
            levelEditor.Disabled = isWalkPathEdit;
        }

        if (isWalkPathEdit)
        {
            HandleWalkPathInput(mouseState, keyState);
        }
        else
        {
            HandleLevelEditorInput();
        }

        base.HandleInput();
    }

    public void HandleLevelEditorInput()
    {
        if (Input.IsKeyJustPressed(Keys.Q))
        {
            levelEditor.SaveLevelEditor("level_editor");
        }
        if (Input.IsKeyJustPressed(Keys.R))
        {
            levelEditor.LoadLevelEditor("level_editor");
        }
    }

    public void HandleWalkPathInput(MouseState mouseState, KeyboardState keyState)
    {
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

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateWalkPathInfo();
    }
}