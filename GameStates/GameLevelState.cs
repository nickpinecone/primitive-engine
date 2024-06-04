using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class GameLevelState : GameState
{
    public static int _gold;
    public static int _hearts;

    public static int Gold
    {
        get { return _gold; }
        set
        {
            _gold = Math.Max(value, 0);
        }
    }

    public static int Hearts
    {
        get { return _hearts; }
        set
        {
            _hearts = value;
            if (_hearts <= 0)
            {
            }
        }
    }

    LevelInfo levelInfo;
    WalkPath walkPath;
    WaveManager waveManager;
    List<Button> skipButtons;

    Label waveNumberLabel;
    Label goldLabel;
    Label heartsLabel;

    int LevelId = 0;
    bool FinalWave = false;

    public GameLevelState(LevelInfo levelInfo, int levelId)
    {
        this.LevelId = levelId;
        this.levelInfo = levelInfo;
    }

    public override void LoadContent(ContentManager contentManager)
    {
        Gold = 200;
        Hearts = 20;

        waveNumberLabel = new Label(null, Vector2.Zero, 0.8f, "Wave: " + 0);
        waveNumberLabel.TextColor = Color.Black;
        AddGameObject(waveNumberLabel);

        goldLabel = new Label(null, Vector2.Zero, 0.8f, "Gold: " + Gold);
        goldLabel.TextColor = Color.Gold;
        AddGameObject(goldLabel);

        heartsLabel = new Label(null, Vector2.Zero, 0.8f, "Hearts: " + Hearts);
        heartsLabel.TextColor = Color.DarkRed;
        AddGameObject(heartsLabel);

        Docker.DockTopRight(heartsLabel, heartsLabel.Size);
        Docker.DockToLeft(goldLabel, goldLabel.Size, heartsLabel, heartsLabel.Size);
        goldLabel.LocalPosition -= new Vector2(16, 0);

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

        AddGameObject(waveManager);
        UpdateWaveLabel(0);

        var worldButton = new Button(null, "World Map", Vector2.Zero, 0.5f);
        worldButton.Interact.OnClick += (_, _) =>
        {
            SwitchState(new WorldMapState());
        };
        Docker.DockBottomRight(worldButton, worldButton.Shape.Size);
        AddGameObject(worldButton);
    }

    private void UpdateWaveLabel(int waveNumber)
    {
        waveNumberLabel.Text = $"Wave: {waveNumber} / {waveManager.MaxWave}";
        Docker.DockTopLeft(waveNumberLabel, waveNumberLabel.Size);
    }

    private void HandleWaveUpdate(object sender, int waveNumber)
    {
        UpdateWaveLabel(waveNumber);

        if (waveNumber > waveManager.MaxWave)
        {
            FinalWave = true;
        }

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
        else if (type == TowerType.Magic)
        {
            var tower = new MagicTower(null, plot, walkPath, plot.WorldPosition, plot.Scale);
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

        goldLabel.Text = "Gold: " + Gold;
        heartsLabel.Text = "Hearts: " + Hearts;

        if (Hearts <= 0)
        {
            SwitchState(new WorldMapState());
        }

        if (FinalWave)
        {
            if (_gameObjects.Where((gameObject) => gameObject is Enemy).Count() <= 0)
            {
                WorldMapState.LevelStates[LevelId] = true;
                WorldMapState.SaveWorldMap();
                SwitchState(new WorldMapState());
            }
        }

        foreach (var button in skipButtons)
        {
            if (!button.Sprite.Hidden)
            {
                button.Label.Text = ((int)waveManager.WaveTimer.TimeLeft).ToString();
            }
        }
    }
}