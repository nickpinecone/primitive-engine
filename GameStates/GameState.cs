using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public abstract class GameState
{
    public event EventHandler OnQuitGame;
    public event EventHandler<GameState> OnSwitchState;

    protected List<GameObject> _gameObjects = new List<GameObject>();
    private List<GameObject> _removeQueue = new List<GameObject>();
    private List<GameObject> _addQueue = new List<GameObject>();

    public abstract void LoadContent(ContentManager contentManager);
    public abstract void UnloadContent(ContentManager contentManager);

    public virtual void HandleInput()
    {
        if (Input.IsKeyJustPressed(Keys.F1))
        {
            GameSettings.IsVisibleCollisions = !GameSettings.IsVisibleCollisions;
        }
        else if (Input.IsKeyJustPressed(Keys.F2))
        {
            GameSettings.CreatorMode = !GameSettings.CreatorMode;
        }

        foreach (var gameObject in _gameObjects)
        {
            gameObject.HandleInput();
        }

        Input.Update();
    }
    public virtual void Update(GameTime gameTime)
    {
        foreach (var gameObject in _removeQueue)
        {
            _gameObjects.Remove(gameObject);
        }
        _removeQueue.Clear();

        foreach (var gameObject in _addQueue)
        {
            _gameObjects.Add(gameObject);
        }
        _addQueue.Clear();

        foreach (var gameObject in _gameObjects)
        {
            gameObject.Update(gameTime);
        }
    }

    protected void SwitchState(GameState gameState)
    {
        OnSwitchState?.Invoke(this, gameState);
    }

    protected void QuitGame()
    {
        OnQuitGame?.Invoke(this, null);
    }

    protected void AddGameObject(GameObject gameObject)
    {
        gameObject.OnQueueFree +=
            (object sender, EventArgs _)
            => RemoveGameObject(gameObject);

        gameObject.OnSpawnObject +=
            (object sender, GameObject spawnObject)
            => AddGameObject(spawnObject);

        _addQueue.Add(gameObject);
    }

    protected void RemoveGameObject(GameObject gameObject)
    {
        _removeQueue.Add(gameObject);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        foreach (var gameObject in _gameObjects.OrderBy((gameObject) => gameObject.ZIndex).ThenBy((gameObject) => gameObject.WorldPosition.Y))
        {
            gameObject.Draw(spriteBatch);
        }
        spriteBatch.End();
    }
}
