using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class GameLevelState : GameState
{
    LevelInfo levelInfo;
    WalkPath walkPath;
    WaveManager waveManager;
    List<Button> skipButtons;
    Label waveNumberLabel;

    public GameLevelState(LevelInfo levelInfo)
    {
        this.levelInfo = levelInfo;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        waveNumberLabel = new Label(null, Vector2.Zero, 1f, "Wave: ");
        waveNumberLabel.TextColor = Color.Black;

        skipButtons = new();
        walkPath = new();
        waveManager = new(null, walkPath);

        waveManager.OnNextWave += HandleNextWave;
        waveManager.OnWaveUpdate += HandleWaveUpdate;

        walkPath.Initialize(levelInfo.WalkPathSave);
        waveManager.Initialize(levelInfo.EnemySave);

        foreach (var node in walkPath.GetStartNodes())
        {
            var source = new Rectangle(455, 400, 175, 175);
            var texture = AssetManager.GetAsset<Texture2D>("GUI/Buttons");
            var skipWaveTime = new Button(null, "0", node.Position + new Vector2(source.Width / 4f, 0), 0.4f, texture, source, Rectangle.Empty, null);
            skipWaveTime.Interact.OnClick += HandleSkipWave;

            skipButtons.Add(skipWaveTime);
            AddGameObject(skipWaveTime);
        }

        LoadLevel(levelInfo.LevelSave);

        AddGameObject(waveNumberLabel);
        AddGameObject(waveManager);
        UpdateWaveLabel(0);
    }

    private void UpdateWaveLabel(int waveNumber)
    {
        waveNumberLabel.Text = $"Wave: {waveNumber} / {waveManager.MaxWave}";
        Docker.DockTopLeft(waveNumberLabel, waveNumberLabel.Size);
    }

    private void HandleWaveUpdate(object sender, int waveNumber)
    {
        UpdateWaveLabel(waveNumber);

        foreach (var button in skipButtons)
        {
            button.Sprite.Hidden = true;
            button.Interact.Disabled = true;
            button.Label.TextColor = Color.Transparent;
        }
    }

    private void HandleNextWave(object sender, EventArgs args)
    {
        foreach (var button in skipButtons)
        {
            button.Sprite.Hidden = false;
            button.Interact.Disabled = false;
            button.Label.TextColor = Color.White;
        }
    }

    private void HandleSkipWave(object sender, EventArgs args)
    {
        waveManager.SkipWait();
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

        foreach (var button in skipButtons)
        {
            if (!button.Sprite.Hidden)
            {
                button.Label.Text = ((int)waveManager.WaveTimer.TimeLeft).ToString();
            }
        }
    }
}