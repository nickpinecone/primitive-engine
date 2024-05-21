
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class GridItem : GameObject
{
    public event EventHandler<Placeable> OnSelect;

    private Placeable _placeable;

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public GridItem(GameObject parent, Sprite sprite, Type type, Vector2 position, float scale) : base(parent)
    {
        _placeable = new Placeable(null, sprite, type, Vector2.Zero, 1f);

        Sprite = new Sprite(this, sprite.Texture, sprite.SourceRectangle, 2);
        Shape = new CollisionShape(this, sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        Interact.OnSelect += HandleSelect;

        AddComponent(Sprite);
        AddComponent(Shape);
        AddComponent(Interact);

        WorldPosition = position;
        Scale = scale;
    }

    public void HandleSelect(object sender, EventArgs args)
    {
        OnSelect?.Invoke(this, _placeable);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}

class Grid : GameObject
{
    public event EventHandler<Placeable> OnItemSelect;

    private List<GridItem> _items;

    public Vector2 Size { get; protected set; }
    public int ColumnAmount { get; protected set; }
    public float SizePerItem { get; protected set; }
    public float Gap { get; protected set; }

    public Grid(GameObject parent, Vector2 size, int columnAmount, float gap) : base(parent)
    {
        Size = size;
        ColumnAmount = columnAmount;
        SizePerItem = Size.X / columnAmount;
        Gap = gap;

        _items = new();
    }

    public Vector2 GetPosition()
    {
        var position = new Vector2(0, 0);
        position.X = (_items.Count % ColumnAmount) * SizePerItem;
        position.Y = (_items.Count / ColumnAmount) * SizePerItem;

        position += new Vector2(SizePerItem, SizePerItem) / 2f;

        return position;
    }

    public void AddItem(Sprite sprite, Type type)
    {
        var wideSide = Math.Max(sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

        var fraction = 1 + Gap / SizePerItem;

        var scale = SizePerItem / (float)wideSide / fraction;
        var gridItem = new GridItem(this, sprite, type, GetPosition(), scale);
        gridItem.OnSelect += HandleItemSelect;

        _items.Add(gridItem);
    }

    public void HandleItemSelect(object sender, Placeable placeable)
    {
        OnItemSelect?.Invoke(this, placeable);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        foreach (var item in _items)
        {
            item.HandleInput();
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        foreach (var item in _items)
        {
            item.Update(gameTime);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (var item in _items)
        {
            item.Draw(spriteBatch);
        }
    }
}