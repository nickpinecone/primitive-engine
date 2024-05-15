using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class LevelEditor : GameObject
{
    public event EventHandler<(Type type, Vector2 position, float scale)> OnItemPlace;

    private GameObject _selectedItem = null;

    private Grid _grid;
    private Texture2D _panel;

    public bool Hidden { get; set; }

    public LevelEditor()
    {
        ZIndex = 1;

        _panel = DebugTexture.GenerateTexture((int)GameSettings.WindowSize.X, (int)GameSettings.WindowSize.Y, Color.White);

        _grid = new(GameSettings.WindowSize, 7, 8);
        _grid.OnItemSelect += HandleItemSelect;

        var towerPlot = new TowerPlot(Vector2.Zero, 1f);
        _grid.AddItem(towerPlot);
    }

    public void HandleItemSelect(object sender, Type type)
    {
        var mouseState = Mouse.GetState();
        var position = mouseState.Position.ToVector2();

        var ctor =
            type.GetConstructor(new Type[] { typeof(Vector2), typeof(float) })
            ?? throw new Exception("Game object does not have an appropriate constructor");
        var gameObject = (GameObject)ctor.Invoke(new object[] { position, 1f });

        _selectedItem = gameObject;
        _selectedItem.AccentColor = Color.White * 0.5f;
    }

    public override void HandleInput()
    {
        if (Input.IsKeyJustPressed(Keys.Z))
        {
            Hidden = !Hidden;
        }

        if (Input.IsMouseJustPressed(MouseButton.Right))
        {
            _selectedItem = null;
        }

        if (Hidden)
        {
            var mouseState = Mouse.GetState();

            if (_selectedItem != null)
            {
                if (Input.IsMouseJustPressed(MouseButton.Left))
                {
                    var position = mouseState.Position.ToVector2();
                    OnItemPlace?.Invoke(this, (_selectedItem.GetType(), position, _selectedItem.Scale));
                }

                var scale = MathHelper.Clamp(mouseState.ScrollWheelValue / 1000f + 1, 0.1f, 10f);
                _selectedItem.Scale = scale;
            }

        }
        else
        {
            _grid.HandleInput();
        }
    }

    public override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();

        if (_selectedItem != null)
        {
            _selectedItem.WorldPosition = mouseState.Position.ToVector2();
        }

        _grid.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDevice)
    {
        if (Hidden)
        {
            _selectedItem?.Draw(spriteBatch, graphicsDevice);
        }
        else
        {
            spriteBatch.Draw(_panel, Vector2.Zero, Color.DarkGray * 0.8f);
            _grid.Draw(spriteBatch, graphicsDevice);
        }
    }
}