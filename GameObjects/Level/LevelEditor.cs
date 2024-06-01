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

class LevelEditor : GameObject
{
    public event EventHandler<bool> OnOverlay;
    public EventHandler<Placeable> OnItemPlace;

    private Placeable _selectedItem;
    private Grid _grid;
    private Texture2D _panel;
    private Button _closeButton;
    private Button _showButton;

    public bool Hidden { get; set; }
    public int SnapAmount { get; set; }

    public LevelEditor(GameObject parent) : base(parent)
    {
        _selectedItem = null;
        _panel = DebugTexture.GenerateRectTexture((int)GameSettings.WindowSize.X, (int)GameSettings.WindowSize.Y, Color.White);
        _grid = new Grid(this, GameSettings.WindowSize, 7, 8);
        _closeButton = new Button(this, "Close", Vector2.Zero, 0.6f);
        _closeButton.Interact.OnClick += (_, _) => { Hidden = true; };
        _grid.LocalPosition += new Vector2(0, _closeButton.Shape.WorldRectangle.Height);
        Docker.DockTopRight(_closeButton, _closeButton.Sprite.Size);

        _showButton = new Button(null, "Show Items", Vector2.Zero, 0.6f);
        _showButton.Interact.OnClick += (_, _) => { Hidden = false; };
        Docker.DockTopRight(_showButton, _showButton.Sprite.Size);

        Hidden = true;
        SnapAmount = 5;
        ZIndex = 1;

        PopulateGrid();
    }

    public void PopulateGrid()
    {
        foreach (var saveable in MetaManager.GetSaveables())
        {
            var type = saveable.GetType();
            var sprite = ((ISaveable)saveable).Sprite;
            if (sprite != null)
            {
                var item = new GridLevelItem(null, sprite, type, Vector2.Zero, 1f);

                item.Interact.OnSelect += (_, _) => HandleItemSelect(item, item.Placeable);

                _grid.AddItem(item, sprite.Size);
            }
        }
    }

    public void HandleItemSelect(object sender, Placeable placeable)
    {
        var copy = placeable.Clone();
        _selectedItem = copy;
        _selectedItem.Sprite.AccentColor = Color.White * 0.5f;
        _selectedItem.Interact.IsSelected = true;
    }

    public override void HandleInput()
    {
        if (EditLevelState.EditState != EditState.LevelEditor) return;

        var mouseState = Mouse.GetState();

        if (Input.IsKeyJustPressed(Keys.Z))
        {
            Hidden = !Hidden;
            OnOverlay?.Invoke(this, !Hidden);
        }

        if (Input.IsKeyJustPressed(Keys.X))
        {
            _selectedItem = null;
        }

        if (Hidden)
        {
            if (EditLevelState.EditState == EditState.LevelEditor)
            {
                _showButton.HandleInput();
            }

            if (_selectedItem != null)
            {
                _selectedItem.HandleInput();

                if (Input.IsMouseJustPressed(MouseButton.Left))
                {
                    OnItemPlace?.Invoke(this, _selectedItem);
                }
            }
        }
        else
        {
            base.HandleInput();
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (EditLevelState.EditState != EditState.LevelEditor) return;

        base.Update(gameTime);

        var mouseState = Mouse.GetState();

        if (_selectedItem != null)
        {
            _selectedItem.Update(gameTime);
            var position = new Vector2((mouseState.Position.X / SnapAmount) * SnapAmount, (mouseState.Position.Y / SnapAmount) * SnapAmount);
            _selectedItem.LocalPosition = position;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Hidden)
        {
            if (EditLevelState.EditState == EditState.LevelEditor)
            {
                _showButton.Draw(spriteBatch);
            }
            _selectedItem?.Draw(spriteBatch);
        }
        else
        {
            spriteBatch.Draw(_panel, Vector2.Zero, Color.DarkGray * 0.8f);
            base.Draw(spriteBatch);
        }
    }
}