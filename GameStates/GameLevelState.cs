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
        MetaManager.LoadWalkPath("walk_path", walkPath);
        walkPath.CalculateLengths();
        LoadLevel("level_editor");
    }

    public void LoadLevel(string filename)
    {
        foreach (var gameObject in MetaManager.LoadLevelEditor(filename))
        {
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