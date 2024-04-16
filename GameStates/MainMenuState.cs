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
        var quitButton = new Button(buttonSprites, new Rectangle(945, 200, 360, 180), "Quit", font, windowMiddle, 1f);

        quitButton.OnClick += HandleQuitButton;

        AddGameObject(quitButton);
    }

    private void HandleQuitButton(object sender, EventArgs args)
    {
        QuitGame();
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