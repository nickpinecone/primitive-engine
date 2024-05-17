using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense;

public static class MetaManager
{
    public static void SaveToFile<T>(List<T> metadata, string filename)
    {
        var data = JsonSerializer.Serialize(metadata);
        var workDir = System.IO.Directory.GetCurrentDirectory();
        File.WriteAllText(workDir + "/Saves/" + filename + ".json", data);
    }

    public static List<T> ReadFromFile<T>(string filename)
    {
        var workDir = System.IO.Directory.GetCurrentDirectory();
        var data = File.ReadAllText(workDir + "/Saves/" + filename + ".json");
        return JsonSerializer.Deserialize<List<T>>(data);
    }

    public static GameObject ConstructObject(Type type, Vector2 position, float scale)
    {
        var ctor =
            type.GetConstructor(new Type[] { typeof(Vector2), typeof(float) })
            ?? throw new Exception("Game object does not have an appropriate constructor");
        return (GameObject)ctor.Invoke(new object[] { position, scale });
    }
}