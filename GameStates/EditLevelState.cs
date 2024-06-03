using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    Button editStateButton;
    Button saveButton;

    LevelInfo levelInfo;
    WalkPath walkPath;
    WalkPathEditor walkPathEditor;
    LevelEditor levelEditor;
    EnemyEditor enemyEditor;

    public EditLevelState(LevelInfo levelInfo)
    {
        this.levelInfo = levelInfo;
    }

    public void UpdateEditStateInfo()
    {
        var text = "Edit: " + EditState switch
        {
            EditState.WalkPath => "Path",
            EditState.LevelEditor => "Level",
            EditState.EnemyEditor => "Enemies",
            _ => ""
        };
        editStateButton.Label.Text = text;
        Docker.DockTopLeft(editStateButton, editStateButton.Sprite.Size);
    }

    public override void LoadContent(ContentManager contentManager)
    {
        levelEditor = new LevelEditor(null);
        levelEditor.OnItemPlace += HandleItemPlace;
        levelEditor.ZIndex = 2;

        walkPath = new();

        walkPathEditor = new WalkPathEditor(null, walkPath);
        walkPathEditor.OnItemPlace += HandleItemPlace;

        LoadEditors();

        enemyEditor = new EnemyEditor(null, walkPath, levelInfo.EnemySave);
        enemyEditor.ZIndex = 2;

        AddGameObject(walkPathEditor);
        AddGameObject(enemyEditor);
        AddGameObject(levelEditor);

        editStateButton = new Button(null, "", Vector2.Zero, 0.5f);
        editStateButton.Interact.OnClick += (_, _) => NextEditState();
        AddGameObject(editStateButton);
        UpdateEditStateInfo();

        saveButton = new Button(null, "Save", Vector2.Zero, editStateButton.Scale);
        saveButton.Interact.OnClick += (_, _) => SaveEditors();
        Docker.DockToRight(saveButton, saveButton.Sprite.Size, editStateButton, editStateButton.Sprite.Size);
        AddGameObject(saveButton);

        var worldButton = new Button(null, "World Map", Vector2.Zero, saveButton.Scale);
        worldButton.Interact.OnClick += (_, _) =>
        {
            SwitchState(new WorldMapState());
        };
        Docker.DockBottomRight(worldButton, worldButton.Shape.Size);
        AddGameObject(worldButton);
    }

    private void NextEditState()
    {
        EditState = (EditState)((int)(EditState + 1) % 3);
        UpdateEditStateInfo();
    }

    private void LoadEditors()
    {
        foreach (var gameObject in MetaManager.LoadLevelEditor(levelInfo.LevelSave))
        {
            var sprite = ((ISaveable)gameObject).Sprite;
            var placeable = new Placeable(gameObject.Parent, sprite, gameObject.GetType(), gameObject.WorldPosition, gameObject.Scale);
            placeable.LocalRotation = gameObject.Rotation;
            AddGameObject(placeable);
        }

        MetaManager.LoadWalkPath(levelInfo.WalkPathSave, walkPath);

        walkPathEditor.GeneratePathNodes();
    }

    private void SaveEditors()
    {
        enemyEditor.WaveManager.IntegrityCheck();

        MetaManager.SaveWalkPath(levelInfo.WalkPathSave, walkPath);

        MetaManager.SaveWaveManager(levelInfo.EnemySave, enemyEditor.WaveManager.NodeWaves);

        MetaManager.SaveLevelEditor(
            levelInfo.LevelSave,
            _gameObjects
                .Where((gameObject) => gameObject is Placeable)
                .Select((gameObject) => (Placeable)gameObject)
                .ToList()
        );
    }

    private void HandleItemPlace(object sender, PathNode node)
    {
        if (node.Node.Type == NodeType.Start)
        {
            walkPath.AddStartNode(node.Node);
            node.Interact.OnDoubleSelect += (_, _) => HandleStartNodeSelect(node, null);
        }
        AddGameObject(node);
    }

    private void HandleItemPlace(object sender, Placeable placeable)
    {
        var copy = placeable.Clone();
        copy.Interact.IsSelected = false;
        Placeable.Selected = null;
        AddGameObject(copy);
    }

    public override void UnloadContent(ContentManager contentManager)
    {
        AssetManager.UnloadAssets();
    }

    public override void HandleInput()
    {
        if (!enemyEditor.Hidden || !levelEditor.Hidden)
        {
            enemyEditor.HandleInput();
            levelEditor.HandleInput();
            // Super dumb, but have to do that because base handle does that
            Input.Update();
            return;
        }

        var keyState = Keyboard.GetState();
        var mouseState = Mouse.GetState();

        if (Input.IsKeyJustPressed(Keys.Escape))
        {
            SwitchState(new WorldMapState());
        }

        else if (Input.IsKeyJustPressed(Keys.W))
        {
            NextEditState();
        }

        else if (Input.IsKeyJustPressed(Keys.Q))
        {
            SaveEditors();
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
    }

    public void HandleLevelEditorInput()
    {
    }

    public void HandleWalkPathInput(MouseState mouseState, KeyboardState keyState)
    {
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