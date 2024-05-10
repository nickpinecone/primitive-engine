using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class GameLevelState : GameState
{
    WalkPath walkPath = new();

    public override void LoadContent(ContentManager contentManager)
    {
        walkPath.LoadFromFile("walk_path");
        walkPath.CalculateLengths();

        var basicOrk = AssetManager.GetAsset<Texture2D>("Enemies/BasicOrk");
        var basicOrkSource = new Rectangle(0, 0, basicOrk.Width, basicOrk.Height);

        var enemy = new Enemy(walkPath, walkPath.GetStartNodes()[0], 24f, 0.4f, basicOrk, basicOrkSource);
        var plot = new TowerPlot(new Vector2(200, 100), 0.8f);

        AddGameObject(enemy);
        AddGameObject(plot);
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
}