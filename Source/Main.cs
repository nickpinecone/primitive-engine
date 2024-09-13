using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Primitive.State;
using Primitive.Utils;

namespace Primitive;

public class Main : Game
{
    private GraphicsDeviceManager _graphics = null;
    private SpriteBatch _spriteBatch = null;
    private BaseState _state = null;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        IsMouseVisible = true;
    }

    public void SwitchState(BaseState state)
    {
        _state = state;

        _state.OnStateSwitch += (_, state) => SwitchState(state);
        _state.Initialize();
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        _spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureHelper.SetGraphics(_graphics);
    }

    protected override void Initialize()
    {
        base.Initialize();

        var screen = new Screen();
        _graphics.PreferredBackBufferWidth = screen.Width;
        _graphics.PreferredBackBufferHeight = screen.Height;
        _graphics.ApplyChanges();

        SwitchState(new GameState());
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        _state.Update(gameTime);
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
