using Microsoft.Xna.Framework;
using NLua;
using Primitive.Utils;

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

    public Script(string name)
    {
        _state = new Lua();
        _filename = Directory.GetCurrentDirectory() + "/Source/Script/" + name + ".lua";

        _state.LoadCLRPackage();
        _state.DoString("import('MonoGame.Framework', 'Microsoft.Xna.Framework')");
        _state.DoString("import('MonoGame.Framework', 'Microsoft.Xna.Framework.Input')");
    }

    public void Initialize()
    {
        _state["screen"] = new Screen();

        var print = typeof(Logger).GetMethod("Print");
        _state.RegisterFunction("print", print);

        _state.DoFile(_filename);
    }

    public void Update(GameTime gameTime)
    {
        (_state["update"] as LuaFunction)?.Call(new object[] { gameTime.ElapsedGameTime.TotalSeconds });
    }
}
