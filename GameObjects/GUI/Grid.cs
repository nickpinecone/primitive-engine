
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class GridLevelItem : GameObject
{
    private Placeable _placeable;

    public Placeable Placeable { get { return _placeable; } }

    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public GridLevelItem(GameObject parent, Sprite sprite, Type type, Vector2 position, float scale) : base(parent)
    {
        _placeable = new Placeable(null, sprite, type, Vector2.Zero, 1f);

        Sprite = new Sprite(this, sprite.Texture, sprite.SourceRectangle, 2);
        Shape = new CollisionShape(this, sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        LocalPosition = position;
        LocalScale = scale;
    }
}

class GridEnemyItem : GameObject
{
    public Sprite Sprite { get; }
    public Type Type { get; set; }

    public InputForm OrderInput { get; }
    public InputForm AmountInput { get; }

    public GridEnemyItem(GameObject parent, Sprite sprite, Type type, Vector2 position, float scale) : base(parent)
    {
        Sprite = new Sprite(this, sprite.Texture, sprite.SourceRectangle, 2);

        OrderInput = new InputForm(this, "Order", Vector2.Zero, 1f);
        AmountInput = new InputForm(this, "Amount", Vector2.Zero, 1f);

        AmountInput.LocalPosition = new Vector2(0, sprite.SourceRectangle.Height * Scale / 2f - AmountInput.Sprite.Size.Y * Scale);
        OrderInput.LocalPosition = AmountInput.LocalPosition - new Vector2(0, OrderInput.Sprite.Size.Y * Scale);

        Type = type;
        LocalPosition = position;
        LocalScale = scale;
    }
}

class Grid : GameObject
{
    private List<GameObject> _items;

    public List<GameObject> Items { get { return _items; } }

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

    public void AddItem(GameObject gameObject, Vector2 size)
    {
        var wideSide = Math.Max(size.X, size.Y);

        var fraction = 1 + Gap / SizePerItem;

        var scale = SizePerItem / (float)wideSide / fraction;

        gameObject.Parent = this;
        gameObject.LocalScale = scale;
        gameObject.LocalPosition = GetPosition();

        _items.Add(gameObject);
    }
}