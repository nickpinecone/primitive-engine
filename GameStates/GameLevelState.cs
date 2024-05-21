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
            AddGameObject(gameObject);
            if (gameObject is TowerPlot plot)
            {
                plot.OnTowerSelect += HandleTowerSelect;
            }
        }
    }

    private void HandleTowerSelect(object sender, TowerType type)
    {
        var plot = (TowerPlot)sender;

        if (type == TowerType.Archer)
        {
            var tower = new ArcherTower(plot, walkPath, 400, plot.WorldPosition, plot.Scale);
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