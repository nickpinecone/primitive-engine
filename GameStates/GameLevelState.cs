using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class GameLevelState : GameState
{
    public override void LoadContent(ContentManager contentManager)
    {
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
            SwitchState(new WorldMapState());
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        base.Draw(spriteBatch, graphicsDevice);
    }
}