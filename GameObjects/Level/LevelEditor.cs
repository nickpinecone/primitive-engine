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

    public bool Hidden { get; set; }
    public bool Disabled { get; set; }
    public int SnapAmount { get; set; }

    public LevelEditor(GameObject parent) : base(parent)
    {
        _selectedItem = null;
        _panel = DebugTexture.GenerateRectTexture((int)GameSettings.WindowSize.X, (int)GameSettings.WindowSize.Y, Color.White);
        _grid = new Grid(this, GameSettings.WindowSize, 7, 8);
        _grid.OnItemSelect += HandleItemSelect;

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
                _grid.AddItem(sprite, type);
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
        if (Disabled) return;

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
        if (Disabled) return;

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
            _selectedItem?.Draw(spriteBatch);
        }
        else
        {
            spriteBatch.Draw(_panel, Vector2.Zero, Color.DarkGray * 0.8f);
            base.Draw(spriteBatch);
        }
    }
}