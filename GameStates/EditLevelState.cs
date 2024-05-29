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

public enum EditState { WalkPath, LevelEditor, EnemyEditor };

public class EditLevelState : GameState
{
    public static EditState EditState = EditState.WalkPath;

    Label editInfo;
    WalkPath walkPath;
    LevelEditor levelEditor;
    EnemyEditor enemyEditor;

    public void UpdateWalkPathInfo()
    {
        var text = "Edit Info: " + EditState switch
        {
            EditState.WalkPath => "Walk Path",
            EditState.LevelEditor => "Level Editor",
            EditState.EnemyEditor => "Enemy Editor",
            _ => ""
        };
        editInfo.Text = text;
        editInfo.LocalPosition = editInfo.TextSize / 2f;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        levelEditor = new LevelEditor(null);
        levelEditor.OnItemPlace += HandleItemPlace;
        levelEditor.ZIndex = 2;

        walkPath = new();
        editInfo = new Label(null, Vector2.Zero, 0.5f, "");
        editInfo.TextColor = Color.Black;
        editInfo.LocalPosition += editInfo.TextSize / 2f;

        LoadEditors();

        enemyEditor = new EnemyEditor(null, walkPath);
        enemyEditor.ZIndex = 2;

        AddGameObject(enemyEditor);
        AddGameObject(levelEditor);
        AddGameObject(editInfo);

        UpdateWalkPathInfo();
    }

    private void LoadEditors()
    {
        foreach (var gameObject in MetaManager.LoadLevelEditor("level_editor"))
        {
            var sprite = ((ISaveable)gameObject).Sprite;
            var placeable = new Placeable(gameObject.Parent, sprite, gameObject.GetType(), gameObject.WorldPosition, gameObject.Scale);
            AddGameObject(placeable);
        }

        MetaManager.LoadWalkPath("walk_path", walkPath);
        GeneratePathNodes();
    }

    private void HandleItemPlace(object sender, Placeable placeable)
    {
        var copy = placeable.Clone();
        copy.Interact.IsSelected = false;
        AddGameObject(copy);
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
            EditState = (EditState)((int)(EditState + 1) % 3);

            UpdateWalkPathInfo();
        }

        switch (EditState)
        {
            case EditState.WalkPath:
                HandleWalkPathInput(mouseState, keyState);
                break;
            case EditState.LevelEditor:
                HandleLevelEditorInput();
                break;
            case EditState.EnemyEditor:
                HandleEnemyEditorInput();
                break;
            default:
                break;
        }

        base.HandleInput();
    }

    private void HandleEnemyEditorInput()
    {
        if (Input.IsKeyJustPressed(Keys.Q))
        {
            MetaManager.SaveWaveManager("enemy_editor", enemyEditor.WaveManager.NodeWaves);
        }
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
            if (pathNode.Node.Type == NodeType.Start)
            {
                pathNode.Interact.OnDoubleSelect += (_, _) => HandleStartNodeSelect(pathNode, null);
            }
            AddGameObject(pathNode);
        }

        if (Input.IsKeyJustPressed(Keys.Q))
        {
            MetaManager.SaveWalkPath("walk_path", walkPath);
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
                if (pathNode.Node.Type == NodeType.Start)
                {
                    pathNode.Interact.OnDoubleSelect += (_, _) => HandleStartNodeSelect(pathNode, null);
                }
                dict[tuple.node] = pathNode;
                AddGameObject(pathNode);
            }

            if (tuple.from != null)
            {
                dict[tuple.from].LinkPath(pathNode);
            }
        }
    }

    private void HandleStartNodeSelect(object sender, EventArgs args)
    {
        var node = (PathNode)sender;

        if (EditState == EditState.EnemyEditor)
        {
            node.Interact.IsSelected = false;
            enemyEditor.Show(node.Node.StartId);
        }
    }
}