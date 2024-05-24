using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class EnemyEditor : GameObject
{
    private GridEnemy _grid;
    private Texture2D _panel;
    private WalkPath _walkPath;

    private Dictionary<GridEnemyItem, Dictionary<int, (int order, int amount)>> _waves;

    private int _prevWave = 0;
    public InputForm WaveInput { get; }

    public Node SelectedNode { get; private set; }
    public bool Hidden { get; private set; }

    public EnemyEditor(GameObject parent, WalkPath walkPath) : base(parent)
    {
        WaveInput = new InputForm(this, "Wave", Vector2.Zero, 1f);
        WaveInput.LocalPosition += new Vector2(
            GameSettings.WindowWidth - WaveInput.Sprite.Size.X / 2f,
            WaveInput.Sprite.Size.Y / 2f
        );

        _panel = DebugTexture.GenerateRectTexture((int)GameSettings.WindowSize.X, (int)GameSettings.WindowSize.Y, Color.White);
        _grid = new GridEnemy(this, GameSettings.WindowSize, 7, 8);
        _grid.LocalPosition += new Vector2(0, WaveInput.Sprite.Size.Y);
        _walkPath = walkPath;
        _waves = new();

        SelectedNode = null;
        Hidden = true;

        PopulateGrid();

        WaveInput.NumberInput.OnValueChange += HandleWaveChange;
    }

    private void HandleWaveChange(object sender, int wave)
    {
        foreach (var item in _grid.Items)
        {
            _waves[item][_prevWave] = (item.OrderInput.NumberInput.Value, item.AmountInput.NumberInput.Value);

            if (!_waves[item].ContainsKey(wave))
            {
                _waves[item][wave] = (0, 0);
            }

            item.OrderInput.NumberInput.Value = _waves[item][wave].order;
            item.AmountInput.NumberInput.Value = _waves[item][wave].amount;
        }

        _prevWave = wave;
    }

    public void PopulateGrid()
    {
        var basicOrk = new BasicOrk(null, _walkPath, SelectedNode, 1f);

        _grid.AddItem(basicOrk.Sprite, basicOrk.GetType());

        foreach (var item in _grid.Items)
        {
            _waves[item] = new();
            _waves[item][_prevWave] = (0, 0);
        }
    }

    public void Show(Node node)
    {
        Hidden = false;
        SelectedNode = node;
    }

    public override void HandleInput()
    {
        if (EditLevelState.EditState != EditState.EnemyEditor) return;

        base.HandleInput();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Hidden) return;

        spriteBatch.Draw(_panel, Vector2.Zero, Color.DarkGray * 0.8f);

        base.Draw(spriteBatch);
    }
}