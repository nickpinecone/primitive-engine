using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json;
using System.Text.Json.Serialization;

using TowerDefense;
using System.IO;

class NodeMetadata
{
    public float X { get; set; }
    public float Y { get; set; }
    public int ID { get; set; }
    public List<int> NextIDs { get; set; } = new();
    public NodeType Type { get; set; } = NodeType.Regular;
}

enum NodeType { Start, End, Regular };

class Node
{
    private List<Node> _nextNodes;

    public Vector2 Position { get; set; }
    public Dictionary<Node, double> PathLengths { get; set; }
    public NodeType Type { get; set; }

    public Node(Vector2 position)
    {
        Position = position;

        _nextNodes = new();
        PathLengths = new();
        Type = NodeType.Regular;
    }

    public List<Node> GetNextNodes()
    {
        return _nextNodes;
    }

    public Node PickNextNode()
    {
        var index = RandomGenerator.Rng.Next(_nextNodes.Count);

        return _nextNodes[index] ?? null;
    }

    public void LinkNode(Node otherNode)
    {
        _nextNodes.Add(otherNode);
    }

    public void UnlinkNode(Node node)
    {
        _nextNodes.Remove(node);
    }
}

class WalkPath
{
    // private Dictionary<string, Node> _enemyNodes;
    private List<Node> _startNodes;

    public WalkPath()
    {
        // _enemyNodes = new();
        _startNodes = new();
    }

    public IEnumerable<(Node node, Node from)> Enumerate()
    {
        Queue<(Node node, Node from)> queue = new();
        HashSet<Node> visited = new();

        foreach (var node in _startNodes)
        {
            queue.Enqueue(new(node, null));
        }

        while (queue.Any())
        {
            var tuple = queue.Dequeue();

            yield return tuple;

            if (!visited.Contains(tuple.node))
            {
                foreach (var nextNode in tuple.node.GetNextNodes())
                {
                    queue.Enqueue(new(nextNode, tuple.node));
                }
                visited.Add(tuple.node);
            }
        }

    }

    public void CalculateLengths()
    {
        foreach (var tuple in Enumerate())
        {
            if (tuple.from != null)
            {
                var distance = (tuple.from.Position - tuple.node.Position).Length();

                tuple.node.PathLengths[tuple.from] = distance;
            }
        }
    }

    public List<Node> GetStartNodes()
    {
        return _startNodes;
    }

    public void AddStartNode(Node node)
    {
        _startNodes.Add(node);
    }

    // public List<string> GetEnemiesToPoints(List<Node> points)
    // {
    //     return new List<string>();
    // }

    // public Node GetNextPoint(string enemy)
    // {
    //     // // Check if enemy has reached the point
    //     // var originNode = _enemyNodes[enemy];
    //     // return originNode.node.PickNextNode();
    //     return new Node();
    // }

    public void SaveToFile(string filename)
    {
        Dictionary<Node, NodeMetadata> dict = new();
        int countID = 0;

        foreach (var tuple in Enumerate())
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

        var data = JsonSerializer.Serialize(dict.Values.ToArray());
        var workDir = System.IO.Directory.GetCurrentDirectory();
        File.WriteAllText(workDir + "/Saves/" + filename + ".json", data);
    }

    public void LoadFromFile(string filename)
    {
        var workDir = System.IO.Directory.GetCurrentDirectory();
        var data = File.ReadAllText(workDir + "/Saves/" + filename + ".json");
        var metadata = JsonSerializer.Deserialize<List<NodeMetadata>>(data);

        // Initializing nodes
        Dictionary<int, Node> dict = new();
        foreach (var meta in metadata)
        {
            var position = new Vector2(meta.X, meta.Y);
            var node = new Node(position) { Type = meta.Type };
            dict[meta.ID] = node;

            if (meta.Type == NodeType.Start)
            {
                _startNodes.Add(node);
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