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

        var playButton = new Button(null, "Play", Vector2.Zero, 0.9f);
        var quitButton = new Button(null, "Quit", Vector2.Zero, 0.9f);
        quitButton.Interact.OnClick += HandleQuitButton;
        playButton.Interact.OnClick += HandlePlayButton;

        var vstack = new Stack(null, windowMiddle, StackDirection.Vertical);
        vstack.AddItem(playButton, playButton.Sprite.Size);
        vstack.AddItem(quitButton, quitButton.Sprite.Size);

        AddGameObject(vstack);
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