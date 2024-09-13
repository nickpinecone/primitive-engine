using Microsoft.Xna.Framework;
using NLua;

public class Script
{
    private Lua _state = null;
    private string _filename = null;

    public Lua State
    {
        get
        {
            return _state;
        }
    }

    public Script(string filename)
    {
        _state = new Lua();
        _filename = Directory.GetCurrentDirectory() + "/Source/Script/" + filename;

        _state.LoadCLRPackage();
        _state.DoString("import('MonoGame.Framework', 'Microsoft.Xna.Framework')");
    }

    public void Initialize()
    {
        _state.DoFile(_filename);
    }

    public void Update(GameTime gameTime)
    {
        (_state["update"] as LuaFunction)?.Call(new object[] { gameTime.ElapsedGameTime.TotalSeconds });
    }
}
