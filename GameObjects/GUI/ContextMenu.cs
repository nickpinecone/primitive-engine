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

    private object _value;
    private Sprite _itemSprite;

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public Label PriceLabel { get; set; }

    public ContextMenuItem(GameObject parent, Sprite itemSprite, object value, Vector2 position, float scale) : base(parent)
    {
        var texture = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
        var source = new Rectangle(75, 735, 65, 55);

        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        Interact.OnDoubleSelect += HandleDoubleSelect;

        _value = value;
        _itemSprite = new Sprite(this, itemSprite.Texture, itemSprite.SourceRectangle);

        var wideSide = Math.Max(itemSprite.SourceRectangle.Width, itemSprite.SourceRectangle.Height);
        var scaleItem = source.Width / (float)wideSide / 1.2f;
        _itemSprite.Scale = scaleItem;

        PriceLabel = new Label(this, new Vector2(0, 18) * scale, 0.4f, "100");
        PriceLabel.TextColor = Color.Yellow;

        WorldPosition = position;
        Scale = scale;
    }

    private void HandleDoubleSelect(object sender, EventArgs args)
    {
        Interact.IsSelected = false;
        OnSelect?.Invoke(this, _value);
    }
}


class ContextMenu : GameObject
{
    public event EventHandler<object> OnSelect;

    private List<ContextMenuItem> _menuItems;

    public float SizePerItem { get; set; }
    public bool Hidden { get; set; }
    public float DistanceAway { get; set; }

    public ContextMenu(GameObject parent, float size) : base(parent)
    {
        _menuItems = new();

        Hidden = true;
        SizePerItem = size;
        DistanceAway = SizePerItem;
    }

    public override void HandleInput()
    {
        if (Hidden) return;

        base.HandleInput();
    }

    private Vector2 GetPosition()
    {
        var y = (_menuItems.Count / 2 - 1) * DistanceAway;
        var x = (_menuItems.Count % 2 - 1) * DistanceAway;

        return new Vector2(x, y);
    }

    public void AddItem(Sprite sprite, object value)
    {
        var wideSide = Math.Max(sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

        var scale = SizePerItem / (float)wideSide;
        var gridItem = new ContextMenuItem(this, sprite, value, GetPosition(), scale);
        gridItem.OnSelect += HandleItemSelect;

        _menuItems.Add(gridItem);
    }

    private void HandleItemSelect(object sender, object value)
    {
        OnSelect?.Invoke(this, value);
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var item in _menuItems)
        {
            if (Hidden && item.Interact.IsSelected)
            {
                Hidden = false;
            }
        }

        if (Hidden) return;

        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Hidden) return;

        base.Draw(spriteBatch);
    }
}