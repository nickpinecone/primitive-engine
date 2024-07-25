﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.State;
using Primitive.UI;

namespace Primitive;

public class Main : Game
{
    private GraphicsDeviceManager _graphics = null;
    private SpriteBatch _spriteBatch = null;
    private BaseState _state = null;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Label.LoadFont(Content);
    }

    public void SwitchState(BaseState state)
    {
        _state = state;

        _state.OnStateSwitch += (_, state) => SwitchState(state);
        _state.Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();

        SwitchState(new MenuState());
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _state.Update(gameTime);
        _state.HandleInput();
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);

        GraphicsDevice.Clear(Color.DimGray);

        _spriteBatch.Begin();
        _state.Draw(_spriteBatch);
        _spriteBatch.End();
    }
}
