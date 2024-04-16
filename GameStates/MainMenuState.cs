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
        var buttonSprites = contentManager.Load<Texture2D>("GUI/Buttons");
        var font = contentManager.Load<SpriteFont>("GUI/MenuFont");

        var windowMiddle = WindowSettings.WindowSize / 2f;
        var offset = new Vector2(0, 120);

        var playButton = new Button(buttonSprites, new Rectangle(945, 200, 360, 180), "Play", font, windowMiddle - offset, 1f);
        var quitButton = new Button(buttonSprites, new Rectangle(945, 200, 360, 180), "Quit", font, windowMiddle + offset, 1f);

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
        contentManager.Unload();
    }

    public override void HandleInput()
    {
        var state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.Escape))
        {
            QuitGame();
        }
        else if (state.IsKeyDown(Keys.Enter))
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