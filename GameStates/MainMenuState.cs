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
    }

    public override void Update(GameTime gameTime)
    {
    }
}