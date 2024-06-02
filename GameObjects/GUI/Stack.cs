using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

enum StackDirection { Horizontal, Vertical };

class Stack : GameObject
{
    private List<Vector2> _sizes { get; set; }

    public List<GameObject> Items { get; private set; }
    public StackDirection Direction { get; private set; }

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
                allSize = _sizes[i].X * Items[i].Scale;
            }

            Items[0].LocalPosition = new Vector2(-allSize / 2f, 0);

            var prevItem = Items[0];
            for (int i = 1; i < Items.Count; i++)
            {
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
                allSize = _sizes[i].Y * Items[i].Scale;
            }

            Items[0].LocalPosition = new Vector2(0, -allSize / 2f);

            var prevItem = Items[0];
            for (int i = 1; i < Items.Count; i++)
            {
                var item = Items[i];
                Docker.DockToBottom(item, _sizes[i - 1], prevItem, _sizes[i]);
                prevItem = item;
            }
        }
    }
}