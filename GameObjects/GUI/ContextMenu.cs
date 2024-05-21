// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Graphics;
// using Microsoft.Xna.Framework.Input;

// using TowerDefense;

// class ContextMenuItem : GameObject
// {
//     public event EventHandler<object> OnSelect;

//     private Selectable _selectable;
//     private object _value;
//     private Sprite _itemSprite { get; set; }

//     public Sprite Sprite { get { return _selectable.Sprite; } }
//     public CollisionShape Shape { get { return _selectable.Shape; } }
//     public bool IsSelected { get { return _selectable.IsSelected; } }

//     public Label PriceLabel { get; set; }

//     public ContextMenuItem(Sprite itemSprite, object value, Vector2 position, float scale)
//     {
//         var sprite = AssetManager.GetAsset<Texture2D>("Towers/ArcherTower");
//         var source = new Rectangle(75, 735, 65, 55);

//         _selectable = new Selectable(Vector2.Zero, 1f, 2, sprite, source, source) { Parent = this };
//         _selectable.OnDoubleSelect += HandleDoubleSelect;
//         _value = value;

//         _itemSprite = itemSprite;
//         _itemSprite.Parent = this;

//         var wideSide = Math.Max(itemSprite.SourceRectangle.Width, itemSprite.SourceRectangle.Height);
//         var scaleItem = source.Width * scale / (float)wideSide / 1.5f;
//         _itemSprite.Scale = scaleItem;

//         PriceLabel = new(new Vector2(0, 18) * scale, 0.4f, "100") { Parent = this };
//         PriceLabel.TextColor = Color.Yellow;

//         WorldPosition = position;
//         Scale = scale;
//     }

//     private void HandleDoubleSelect(object sender, EventArgs args)
//     {
//         _selectable.IsSelected = false;
//         OnSelect?.Invoke(this, _value);
//     }

//     public override void HandleInput()
//     {
//         _selectable.HandleInput();
//     }

//     public override void Update(GameTime gameTime)
//     {
//         _selectable.Update(gameTime);
//     }

//     public override void Draw(SpriteBatch spriteBatch)
//     {
//         _selectable.Draw(spriteBatch);
//         _itemSprite.Draw(spriteBatch);
//         PriceLabel.Draw(spriteBatch);
//     }
// }


// class ContextMenu : GameObject
// {
//     public event EventHandler<object> OnSelect;

//     private List<ContextMenuItem> _menuItems;

//     public float SizePerItem { get; set; }
//     public bool Hidden { get; set; }

//     public ContextMenu(float size)
//     {
//         _menuItems = new();

//         Hidden = true;
//         SizePerItem = size;
//     }

//     public override void HandleInput()
//     {
//         if (Hidden) return;

//         foreach (var item in _menuItems)
//         {
//             item.HandleInput();
//         }
//     }

//     private Vector2 GetPosition()
//     {
//         var y = _menuItems.Count / 2 - 1 * SizePerItem;
//         var x = _menuItems.Count % 2 - 1 * SizePerItem;

//         return new Vector2(x, y);
//     }

//     public void AddItem(Sprite sprite, object value)
//     {
//         var wideSide = Math.Max(sprite.SourceRectangle.Width, sprite.SourceRectangle.Height);

//         var scale = SizePerItem / (float)wideSide;
//         var gridItem = new ContextMenuItem(sprite, value, GetPosition(), scale) { Parent = this };
//         gridItem.OnSelect += HandleItemSelect;

//         _menuItems.Add(gridItem);
//     }

//     private void HandleItemSelect(object sender, object value)
//     {
//         OnSelect?.Invoke(this, value);
//     }

//     public override void Update(GameTime gameTime)
//     {
//         foreach (var item in _menuItems)
//         {
//             if (Hidden && item.IsSelected)
//             {
//                 Hidden = false;
//             }
//         }

//         if (Hidden) return;

//         foreach (var item in _menuItems)
//         {
//             item.Update(gameTime);
//         }
//     }

//     public override void Draw(SpriteBatch spriteBatch)
//     {
//         if (Hidden) return;

//         foreach (var item in _menuItems)
//         {
//             item.Draw(spriteBatch);
//         }
//     }
// }