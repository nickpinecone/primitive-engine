using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class GridEnemyItem : GameObject
{
    public Sprite Sprite { get; }
    public Type Type { get; set; }

    // private int _orderValue;
    // public int OrderValue
    // {
    //     get { return _orderValue; }
    //     private set
    //     {
    //         _orderValue = value;
    //         AdjustOrderLable();
    //     }
    // }

    // private int _amountValue;
    // public int AmountValue
    // {
    //     get { return _amountValue; }
    //     private set
    //     {
    //         _amountValue = value;
    //         AdjustAmountValue();
    //     }
    // }

    // public Label OrderLabel { get; }
    // public Button OrderDecrease { get; }
    // public Button OrderIncrease { get; }

    // public Label AmountLabel { get; }
    // public Button AmountDecrease { get; }
    // public Button AmountIncrease { get; }

    public GridEnemyItem(GameObject parent, Sprite sprite, Type type, Vector2 position, float scale) : base(parent)
    {
        Sprite = new Sprite(this, sprite.Texture, sprite.SourceRectangle, 2);

        var source = new Rectangle(0, 0, 30, 30);
        var backTexture = DebugTexture.GenerateRectTexture(source.Width, source.Height, Color.Gray);

        // OrderLabel = new Label(this, Vector2.Zero, 1f, "0");
        // OrderDecrease = new Button(this, "-", Vector2.Zero, 1f, backTexture, source, Rectangle.Empty, null);
        // OrderDecrease.Interact.OnClick += (_, _) => OrderValue--;
        // OrderIncrease = new Button(this, "+", Vector2.Zero, 1f, backTexture, source, Rectangle.Empty, null);
        // OrderIncrease.Interact.OnClick += (_, _) => OrderValue++;

        // AmountLabel = new Label(this, Vector2.Zero, 1f, "0");
        // AmountDecrease = new Button(this, "-", Vector2.Zero, 1f, backTexture, source, Rectangle.Empty, null);
        // AmountDecrease.Interact.OnClick += (_, _) => AmountValue--;
        // AmountIncrease = new Button(this, "+", Vector2.Zero, 1f, backTexture, source, Rectangle.Empty, null);
        // AmountIncrease.Interact.OnClick += (_, _) => AmountValue++;

        LocalPosition = position;
        LocalScale = scale;
    }

    // private void AdjustOrderLable()
    // {
    //     OrderLabel.Text = OrderValue.ToString();
    // }

    // private void AdjustAmountValue()
    // {
    //     AmountLabel.Text = AmountValue.ToString();
    // }
}

class GridEnemy : GameObject
{
    public event EventHandler<Placeable> OnItemSelect;

    private List<GridEnemyItem> _items;

    public Vector2 Size { get; protected set; }
    public int ColumnAmount { get; protected set; }
    public float SizePerItem { get; protected set; }
    public float Gap { get; protected set; }

    public GridEnemy(GameObject parent, Vector2 size, int columnAmount, float gap) : base(parent)
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
        var gridItem = new GridEnemyItem(this, sprite, type, GetPosition(), scale);
        // gridItem.OnSelect += HandleItemSelect;

        _items.Add(gridItem);
    }

    public void HandleItemSelect(object sender, Placeable placeable)
    {
        OnItemSelect?.Invoke(this, placeable);
    }
}