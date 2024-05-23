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
        levelEditor = new LevelEditor(null);
        levelEditor.OnOverlay += HandleOverlay;
        levelEditor.OnItemPlace += HandleItemPlace;

        AddGameObject(levelEditor);

        // Walk Path Debug Info
        walkPath = new();
        isWalkPathEdit = false;
        editInfo = new Label(null, Vector2.Zero, 0.5f, "");
        editInfo.TextColor = Color.Black;
        editInfo.LocalPosition += editInfo.TextSize / 2f;

        UpdateWalkPathInfo();
        AddGameObject(editInfo);
    }

    private void HandleItemPlace(object sender, Placeable placeable)
    {
        var copy = placeable.Clone();
        copy.Interact.IsSelected = false;
        AddGameObject(copy);
    }

    private void HandleOverlay(object sender, bool isOpen)
    {
        PathNode.Hidden = isOpen;
        Placeable.Disabled = isOpen;
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
            Placeable.Disabled = isWalkPathEdit;
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
            MetaManager.SaveLevelEditor(
                "level_editor",
                _gameObjects
                    .Where((gameObject) => gameObject is Placeable)
                    .Select((gameObject) => (Placeable)gameObject)
                    .ToList()
            );
        }
        if (Input.IsKeyJustPressed(Keys.R))
        {
            foreach (var gameObject in MetaManager.LoadLevelEditor("level_editor"))
            {
                var sprite = ((ISaveable)gameObject).Sprite;
                var placeable = new Placeable(gameObject.Parent, sprite, gameObject.GetType(), gameObject.WorldPosition, gameObject.Scale);
                AddGameObject(placeable);
            }
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

            var pathNode = new PathNode(null, node, nodeType);
            AddGameObject(pathNode);
        }

        if (Input.IsKeyJustPressed(Keys.Q))
        {
            MetaManager.SaveWalkPath("walk_path", walkPath);
        }
        if (Input.IsKeyJustPressed(Keys.R))
        {
            MetaManager.LoadWalkPath("walk_path", walkPath);
            GeneratePathNodes();
        }
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
                pathNode = new PathNode(null, tuple.node, tuple.node.Type);
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