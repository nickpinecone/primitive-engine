using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class WorldMapState : GameState
{
    public static List<bool> LevelStates = new();

    public override void LoadContent(ContentManager contentManager)
    {
        LevelPoint.LevelCount = 0;

        var creatorButton = new Button(
            null,
            "Creator: " + (GameSettings.CreatorMode ? "On" : "Off"),
            Vector2.Zero, 0.5f
        );
        creatorButton.Interact.OnClick += (_, _) =>
        {
            GameSettings.CreatorMode = !GameSettings.CreatorMode;
            creatorButton.Label.Text = "Creator: " + (GameSettings.CreatorMode ? "On" : "Off");
        };
        Docker.DockBottomLeft(creatorButton, creatorButton.Sprite.Size);
        AddGameObject(creatorButton);

        var closeButton = new Button(null, "Main Menu", new Vector2(GameSettings.WindowWidth, 0), creatorButton.Scale);
        closeButton.Interact.OnClick += (_, _) =>
        {
            SwitchState(new MainMenuState());
        };
        Docker.DockBottomRight(closeButton, closeButton.Sprite.Size);
        AddGameObject(closeButton);

        LoadWorldMap();
    }

    private void LoadWorldMap()
    {
        var positions = new Vector2[] {
            new Vector2(200, 500),
            new Vector2(400, 350),
            new Vector2(600, 200),
            new Vector2(800, 200),
            new Vector2(1000, 100),
        };

        LevelStates.Clear();
        var states = MetaManager.LoadWorldMap("worldmap") ?? (new bool[positions.Length]).ToList();
        var firstUncomplete = true;
        for (int i = 0; i < positions.Length; i++)
        {
            var levelPoint = new LevelPoint(null, positions[i], 1f);
            levelPoint.Completed = states[i];
            LevelStates.Add(states[i]);

            PlaceLevelPoint(levelPoint);

            if (!levelPoint.Completed && firstUncomplete)
            {
                firstUncomplete = false;
            }
            else if (!levelPoint.Completed)
            {
                levelPoint.Interact.Disabled = true;
                levelPoint.Sprite.Hidden = true;
            }
        }
    }

    public static void SaveWorldMap()
    {
        MetaManager.SaveWorldMap("worldmap", LevelStates);
    }

    private void PlaceLevelPoint(LevelPoint levelPoint)
    {
        levelPoint.OnLevelSelect += HandleLevelSelect;
        AddGameObject(levelPoint);
        LevelPoint.LevelCount += 1;
    }

    public override void UnloadContent(ContentManager contentManager)
    {
        AssetManager.UnloadAssets();
    }

    public void HandleLevelSelect(object sender, EventArgs args)
    {
        var levelPoint = (LevelPoint)sender;

        if (GameSettings.CreatorMode)
        {
            SwitchState(new EditLevelState(levelPoint.LevelInfo));
        }
        else
        {
            SwitchState(new GameLevelState(levelPoint.LevelInfo, levelPoint.LevelNumber));
        }
    }

    public override void HandleInput()
    {
        var state = Keyboard.GetState();

        if (Input.IsKeyJustPressed(Keys.Escape))
        {
            SwitchState(new MainMenuState());
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}