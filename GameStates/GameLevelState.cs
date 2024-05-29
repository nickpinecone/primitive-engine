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
    WalkPath walkPath;
    WaveManager waveManager;

    public override void LoadContent(ContentManager contentManager)
    {
        walkPath = new();
        waveManager = new(null, walkPath);

        waveManager.Initialize();
        walkPath.Initialize();

        LoadLevel("level_editor");

        AddGameObject(waveManager);
    }

    public void LoadLevel(string filename)
    {
        // TODO dont have to do this anymore
        foreach (var gameObject in MetaManager.LoadLevelEditor(filename))
        {
            AddGameObject(gameObject);
            if (gameObject is TowerPlot plot)
            {
                plot.OnTowerSelect += HandleTowerSelect;
            }
        }
    }

    // TODO this as well
    private void HandleTowerSelect(object sender, TowerType type)
    {
        var plot = (TowerPlot)sender;

        if (type == TowerType.Archer)
        {
            var tower = new ArcherTower(null, plot, walkPath, plot.WorldPosition, plot.Scale);
            AddGameObject(tower);
        }
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