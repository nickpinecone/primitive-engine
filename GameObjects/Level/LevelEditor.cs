using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class ObjectMetadata
{
    public string TypeName { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Scale { get; set; }
}

class LevelEditor : GameObject
{
    public event EventHandler<(Type type, Vector2 position, float scale)> OnItemPlace;

    private GameObject _selectedItem = null;
    private List<GameObject> _placedObjects = new();

    private Grid _grid;
    private Texture2D _panel;

    public bool Hidden { get; set; }

    public LevelEditor()
    {
        Hidden = true;

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
        var mouseState = Mouse.GetState();
        foreach (var gameObject in _placedObjects)
        {
            if (Input.IsMouseJustPressed(MouseButton.Right))
            {
                if (gameObject.WorldRectangle.Contains(mouseState.Position))
                {
                    _placedObjects.Remove(gameObject);
                    break;
                }
            }
            else if (Input.IsMouseJustPressed(MouseButton.Left))
            {
                if (_selectedItem == null && gameObject.WorldRectangle.Contains(mouseState.Position))
                {
                    _selectedItem = gameObject;
                    _placedObjects.Remove(gameObject);
                    return;
                }
            }
        }

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
            if (_selectedItem != null)
            {
                if (Input.IsMouseJustPressed(MouseButton.Left))
                {
                    var position = mouseState.Position.ToVector2();
                    PlaceItem((_selectedItem.GetType(), position, _selectedItem.Scale));
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

    public void PlaceItem((Type type, Vector2 position, float scale) data)
    {
        var ctor =
            data.type.GetConstructor(new Type[] { typeof(Vector2), typeof(float) })
            ?? throw new Exception("Game object does not have an appropriate constructor");
        var gameObject = (GameObject)ctor.Invoke(new object[] { data.position, data.scale });

        _placedObjects.Add(gameObject);
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
        foreach (var gameObject in _placedObjects)
        {
            gameObject.Draw(spriteBatch, graphicsDevice);
        }

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

    public void SaveLevelEditor(string filename)
    {
        var metadata = new List<ObjectMetadata>();

        foreach (var gameObject in _placedObjects)
        {
            var meta = new ObjectMetadata()
            {
                TypeName = gameObject.GetType().FullName,
                X = gameObject.WorldPosition.X,
                Y = gameObject.WorldPosition.Y,
                Scale = gameObject.Scale,
            };

            metadata.Add(meta);
        }

        var data = JsonSerializer.Serialize(metadata);
        var workDir = System.IO.Directory.GetCurrentDirectory();
        File.WriteAllText(workDir + "/Saves/" + filename + ".json", data);
    }

    public void LoadLevelEditor(string filename)
    {
        var workDir = System.IO.Directory.GetCurrentDirectory();
        var data = File.ReadAllText(workDir + "/Saves/" + filename + ".json");
        var metadata = JsonSerializer.Deserialize<List<ObjectMetadata>>(data);

        foreach (var meta in metadata)
        {
            Type type = Type.GetType(meta.TypeName);
            var position = new Vector2(meta.X, meta.Y);

            var ctor =
                type.GetConstructor(new Type[] { typeof(Vector2), typeof(float) })
                ?? throw new Exception("Game object does not have an appropriate constructor");
            var gameObject = (GameObject)ctor.Invoke(new object[] { position, meta.Scale });

            _placedObjects.Add(gameObject);
        }
    }
}