using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense;

public interface ISaveable
{
    public Sprite Sprite { get; }
}

class ObjectMetadata
{
    public string TypeName { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Scale { get; set; }
}

class NodeMetadata
{
    public float X { get; set; }
    public float Y { get; set; }
    public int ID { get; set; }
    public List<int> NextIDs { get; set; } = new();
    public NodeType Type { get; set; } = NodeType.Regular;
}

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
            type.GetConstructor(new Type[] { typeof(GameObject), typeof(Vector2), typeof(float) })
            ?? throw new Exception("Game object does not have an appropriate constructor");
        return (GameObject)ctor.Invoke(new object[] { null, position, scale });
    }

    public static IEnumerable<GameObject> GetSaveables()
    {
        var saveables =
            AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany((assembly) => assembly.GetTypes())
            .Where((type) => type.GetInterface(typeof(ISaveable).Name) != null && !type.IsAbstract);

        foreach (var type in saveables)
        {
            yield return MetaManager.ConstructObject(type, Vector2.Zero, 1f);
        }
    }

    public static void SaveLevelEditor(string filename, List<Placeable> placeables)
    {
        var metadata = new List<ObjectMetadata>();

        foreach (var placeable in placeables)
        {
            var meta = new ObjectMetadata()
            {
                TypeName = placeable.Type.FullName,
                X = placeable.WorldPosition.X,
                Y = placeable.WorldPosition.Y,
                Scale = placeable.Scale,
            };

            metadata.Add(meta);
        }

        MetaManager.SaveToFile(metadata, filename);
    }

    public static List<GameObject> LoadLevelEditor(string filename)
    {
        var gameObjects = new List<GameObject>();
        var metadata = MetaManager.ReadFromFile<ObjectMetadata>(filename);

        foreach (var meta in metadata)
        {
            Type type = Type.GetType(meta.TypeName);
            var position = new Vector2(meta.X, meta.Y);

            var gameObject = MetaManager.ConstructObject(type, position, meta.Scale);
            gameObjects.Add(gameObject);
        }

        return gameObjects;
    }

    public static void SaveWalkPath(string filename, WalkPath walkPath)
    {
        Dictionary<Node, NodeMetadata> dict = new();
        int countID = 0;

        foreach (var tuple in walkPath.Enumerate())
        {
            if (!dict.ContainsKey(tuple.node))
            {
                var newMeta = new NodeMetadata()
                {
                    X = tuple.node.Position.X,
                    Y = tuple.node.Position.Y,
                    ID = countID,
                    Type = tuple.node.Type,
                };

                dict[tuple.node] = newMeta;

                countID++;
            }

            var meta = dict[tuple.node];

            if (tuple.from != null)
            {
                dict[tuple.from].NextIDs.Add(meta.ID);
            }
        }

        MetaManager.SaveToFile(dict.Values.ToList(), filename);
    }

    public static void LoadWalkPath(string filename, WalkPath walkPath)
    {
        var metadata = MetaManager.ReadFromFile<NodeMetadata>(filename);

        // Initializing nodes
        Dictionary<int, Node> dict = new();
        foreach (var meta in metadata)
        {
            var position = new Vector2(meta.X, meta.Y);
            var node = new Node(position) { Type = meta.Type };
            dict[meta.ID] = node;

            if (meta.Type == NodeType.Start)
            {
                walkPath.AddStartNode(node);
            }
        }

        // Linking nodes
        foreach (var meta in metadata)
        {
            var node = dict[meta.ID];
            foreach (var other in meta.NextIDs)
            {
                node.LinkNode(dict[other]);
            }
        }
    }
}