using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class ContextMenuItem : GameObject
{
    public event EventHandler<object> OnSelect;

    private Selectable _selectable;
    private object _value;

    public Sprite Sprite { get { return _selectable.Sprite; } }
    public CollisionShape Shape { get { return _selectable.Shape; } }

    public ContextMenuItem(Sprite sprite, object value, Vector2 position, float scale)
    {
        _selectable = new Selectable(Vector2.Zero, 1f, 2, sprite.Texture, sprite.SourceRectangle, sprite.SourceRectangle) { Parent = this };
        _selectable.OnDoubleSelect += HandleDoubleSelect;
        _value = value;
        WorldPosition = position;
        Scale = scale;
    }

    private void HandleDoubleSelect(object sender, EventArgs args)
    {
        OnSelect?.Invoke(this, _value);
    }

    public override void HandleInput()
    {
        _selectable.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        _selectable.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _selectable.Draw(spriteBatch);
    }
}


class ContextMenu : GameObject
{
    public event EventHandler<object> OnSelect;

    private List<ContextMenuItem> _menuItems;

    public Sprite Sprite { get; set; }

    public float SizePerItem { get; set; }
    public bool Hidden { get; set; }

    public override void HandleInput()
    {
        if (Hidden) return;

        foreach (var item in _menuItems)
        {
            item.HandleInput();
        }
    }

    public ContextMenu(float size)
    {
        _menuItems = new();

        var sprite = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var source = new Rectangle(70, 710, 185, 180);

        Sprite = new(sprite, source) { Parent = this };

        Hidden = true;
        SizePerItem = size;
    }

    private Vector2 GetPosition()
    {
        var y = _menuItems.Count / 2 - 1 * 45;
        var x = _menuItems.Count % 2 - 1 * 55;

        return new Vector2(x, y);
    }

    public void AddItem(Sprite sprite, object value)
    {
        var wideSide = Math.Max(sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

        var scale = SizePerItem / (float)wideSide;
        var gridItem = new ContextMenuItem(sprite, value, GetPosition(), scale) { Parent = this };
        gridItem.OnSelect += HandleItemSelect;

        _menuItems.Add(gridItem);
    }

    private void HandleItemSelect(object sender, object value)
    {
        OnSelect?.Invoke(this, value);
    }

    public override void Update(GameTime gameTime)
    {
        if (Hidden) return;

        foreach (var item in _menuItems)
        {
            item.Update(gameTime);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Hidden) return;

        Sprite.Draw(spriteBatch);

        foreach (var item in _menuItems)
        {
            item.Draw(spriteBatch);
        }
    }
}