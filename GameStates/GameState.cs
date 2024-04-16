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

    public abstract void LoadContent(ContentManager contentManager);
    public abstract void UnloadContent(ContentManager contentManager);

    public abstract void HandleInput();
    public abstract void Update(GameTime gameTime);

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
        _gameObjects.Add(gameObject);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        foreach (var gameObject in _gameObjects.OrderBy((gameObject) => gameObject.WorldPosition.Y))
        {
            spriteBatch.Draw(gameObject.Texture, gameObject.WorldPosition, gameObject.SourceRectangle, Color.White);
        }
        spriteBatch.End();
    }
}
