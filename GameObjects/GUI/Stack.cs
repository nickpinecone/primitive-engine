using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public enum StackDirection { Horizontal, Vertical };

public class Stack : GameObject
{
    private List<Vector2> _sizes { get; set; }

    public List<GameObject> Items { get; private set; }
    public StackDirection Direction { get; private set; }
    public Vector2 Size { get; private set; }

    public Stack(GameObject parent, Vector2 position, StackDirection stackDirection) : base(parent)
    {
        _sizes = new();
        Items = new();

        Direction = stackDirection;
        LocalPosition = position;
    }

    public void AddItem(GameObject gameObject, Vector2 size)
    {
        gameObject.Parent = this;
        Items.Add(gameObject);
        _sizes.Add(size);
        PositionItems();
    }

    private void PositionItems()
    {
        if (Direction == StackDirection.Horizontal)
        {
            float allSize = 0f;
            for (int i = 0; i < Items.Count; i++)
            {
                allSize += _sizes[i].X * Items[i].Scale;
            }
            Size = new Vector2(allSize, _sizes[0].Y * Items[0].Scale);

            Items[0].LocalPosition = new Vector2(-allSize / 2f, 0);
            Items[0].LocalPosition += new Vector2(_sizes[0].X * Items[0].Scale / 2f, 0);

            var prevItem = Items[0];
            for (int i = 1; i < Items.Count; i++)
            {
                if (_sizes[i].Y > Size.Y)
                {
                    Size = new Vector2(Size.X, _sizes[i].Y * Items[i].Scale);
                }

                var item = Items[i];
                Docker.DockToRight(item, _sizes[i - 1], prevItem, _sizes[i]);
                prevItem = item;
            }
        }
        else
        {
            float allSize = 0f;
            for (int i = 0; i < Items.Count; i++)
            {
                allSize += _sizes[i].Y * Items[i].Scale;
            }
            Size = new Vector2(_sizes[0].X, allSize);

            Items[0].LocalPosition = new Vector2(0, -allSize / 2f);
            Items[0].LocalPosition += new Vector2(0, _sizes[0].Y * Items[0].Scale / 2f);

            var prevItem = Items[0];
            for (int i = 1; i < Items.Count; i++)
            {
                if (_sizes[i].X > Size.X)
                {
                    Size = new Vector2(_sizes[i].X * Items[i].Scale, Size.Y);
                }

                var item = Items[i];
                Docker.DockToBottom(item, _sizes[i - 1], prevItem, _sizes[i]);
                prevItem = item;
            }
        }
    }
}