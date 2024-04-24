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
        var flag = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var flagSource = new Rectangle(1120, 675, 75, 150);

        var levelPoint = new Selectable(new Vector2(100, 100), 0.5f, 2, flag, flagSource, flagSource);

        AddGameObject(levelPoint);
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
            SwitchState(new MainMenuState());
        }
        if (Input.IsKeyJustPressed(Keys.Enter))
        {
            SwitchState(new GameLevelState());
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}