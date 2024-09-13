using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Primitive.Utils;

public static class Logger
{
    static public void Print(params object[] others)
    {
        foreach (object obj in others)
        {
            Console.WriteLine(obj.ToString());
        }
    }

    public static void Log(string text, [CallerFilePath] string file = "", [CallerMemberName] string member = "",
                           [CallerLineNumber] int line = 0)
    {
        Console.WriteLine("{0} at {1} line {2}: {3}", Path.GetFileName(file), member, line, text);
    }

    public static void Log(Vector2 vector, string label)
    {
        Log($"{label}: " + $"X {vector.X}, Y {vector.Y}");
    }
}
