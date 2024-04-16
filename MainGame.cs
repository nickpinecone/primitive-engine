using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameState _gameState;

    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected void HandleQuitGame(object sender, EventArgs args)
    {
        Exit();
    }

    protected void SwitchState(GameState newState)
    {
        if (_gameState != null)
        {
            _gameState.OnSwitchState -= HandleSwitchState;
            _gameState.OnQuitGame -= HandleQuitGame;
            _gameState.UnloadContent(Content);
        }

        _gameState = newState;

        _gameState.LoadContent(Content);

        _gameState.OnSwitchState += HandleSwitchState;
        _gameState.OnQuitGame += HandleQuitGame;
    }

    protected void HandleSwitchState(object sender, GameState newState)
    {
        SwitchState(newState);
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        SwitchState(new MainMenuState());

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        _gameState.HandleInput();
        _gameState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        _gameState.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}
