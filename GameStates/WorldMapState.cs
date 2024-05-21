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
        var levelPoint = new LevelPoint(null, new Vector2(100, 100), 1f, null);
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