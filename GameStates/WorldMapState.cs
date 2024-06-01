using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class WorldMapState : GameState
{
    public override void LoadContent(ContentManager contentManager)
    {
        var creatorButton = new Button(
            null,
            "Creator: " + (GameSettings.CreatorMode ? "On" : "Off"),
            Vector2.Zero, 0.6f
        );
        creatorButton.Interact.OnClick += (_, _) =>
        {
            GameSettings.CreatorMode = !GameSettings.CreatorMode;
            creatorButton.Label.Text = "Creator: " + (GameSettings.CreatorMode ? "On" : "Off");
        };
        Docker.DockTopLeft(creatorButton, creatorButton.Sprite.Size);
        AddGameObject(creatorButton);

        var closeButton = new Button(null, "Main Menu", new Vector2(GameSettings.WindowWidth, 0), 0.6f);
        closeButton.Interact.OnClick += (_, _) =>
        {
            SwitchState(new MainMenuState());
        };
        Docker.DockTopRight(closeButton, closeButton.Sprite.Size);
        AddGameObject(closeButton);

        var levelPoint = new LevelPoint(null, new Vector2(100, 300), 1f, null);
        levelPoint.OnLevelSelect += HandleLevelSelect;
        AddGameObject(levelPoint);
    }

    public override void UnloadContent(ContentManager contentManager)
    {
        AssetManager.UnloadAssets();
    }

    public void HandleLevelSelect(object sender, GameState level)
    {
        if (GameSettings.CreatorMode)
        {
            SwitchState(new EditLevelState());
        }
        else
        {
            SwitchState(new GameLevelState());
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