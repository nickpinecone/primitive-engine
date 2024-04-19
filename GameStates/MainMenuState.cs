using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class MainMenuState : GameState
{
    public override void LoadContent(ContentManager contentManager)
    {
        var windowMiddle = GameSettings.WindowSize / 2f;
        var offset = new Vector2(0, 120);

        var playButton = new Button("Play", windowMiddle - offset);
        var quitButton = new Button("Quit", windowMiddle + offset);

        quitButton.OnClick += HandleQuitButton;
        playButton.OnClick += HandlePlayButton;

        AddGameObject(quitButton);
        AddGameObject(playButton);
    }

    private void HandleQuitButton(object sender, EventArgs args)
    {
        QuitGame();
    }

    private void HandlePlayButton(object sender, EventArgs args)
    {
        SwitchState(new WorldMapState());
    }

    public override void UnloadContent(ContentManager contentManager)
    {
        AssetManager.UnloadAssets();
    }

    public override void HandleInput()
    {
        var state = Keyboard.GetState();

        if (Input.IsKeyJustPressed(Keys.Escape))
        {
            QuitGame();
        }
        else if (Input.IsKeyJustPressed(Keys.Enter))
        {
            SwitchState(new WorldMapState());
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}