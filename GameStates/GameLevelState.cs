using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
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
        LoadLevel("level_editor");

        // var basicOrk = AssetManager.GetAsset<Texture2D>("Enemies/BasicOrk");
        // var basicOrkSource = new Rectangle(0, 0, basicOrk.Width, basicOrk.Height);

        // var enemy = new Enemy(walkPath, walkPath.GetStartNodes()[0], 24f, 100, 0.4f, basicOrk, basicOrkSource);
        // var plot = new TowerPlot(new Vector2(200, 100), 0.8f);
        // plot.OnTowerSelect += HandleTowerSelect;

        // AddGameObject(enemy);
        // AddGameObject(plot);
    }

    public void LoadLevel(string filename)
    {
        var workDir = System.IO.Directory.GetCurrentDirectory();
        var data = File.ReadAllText(workDir + "/Saves/" + filename + ".json");
        var metadata = JsonSerializer.Deserialize<List<ObjectMetadata>>(data);

        foreach (var meta in metadata)
        {
            Type type = Type.GetType(meta.TypeName);
            var position = new Vector2(meta.X, meta.Y);

            var ctor =
                type.GetConstructor(new Type[] { typeof(Vector2), typeof(float) })
                ?? throw new Exception("Game object does not have an appropriate constructor");
            var gameObject = (GameObject)ctor.Invoke(new object[] { position, meta.Scale });

            if (gameObject is TowerPlot towerPlot)
            {
                towerPlot.OnTowerSelect += HandleTowerSelect;
            }

            AddGameObject(gameObject);
        }
    }

    public override void UnloadContent(ContentManager contentManager)
    {
        AssetManager.UnloadAssets();
    }

    public void HandleTowerSelect(object sender, EventArgs args)
    {
        var archerTower = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var archerTowerSource = new Rectangle(65, 180, 150, 190);

        var plot = (TowerPlot)sender;
        var tower = new Tower(plot, walkPath, 400, plot.WorldPosition, plot.Scale, archerTower, archerTowerSource);

        AddGameObject(tower);
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