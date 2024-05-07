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

class Node
{
    private List<Node> _nextNodes;

    [JsonIgnore]
    public Vector2 Position { get; set; }
    [JsonIgnore]
    public Dictionary<Node, double> PathLengths { get; set; }

    // Json Serialization
    public List<Node> NextNodes
    {
        get { return _nextNodes; }
        set { _nextNodes = value; }
    }

    public float X
    {
        get { return Position.X; }
        set { Position = new Vector2(value, Position.Y); }
    }

    public float Y
    {
        get { return Position.Y; }
        set { Position = new Vector2(Position.X, value); }
    }

    public Node(Vector2 position)
    {
        _nextNodes = new();
        PathLengths = new();
        Position = position;
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

    public void CalculateLengths()
    {
        Queue<(Node node, Node from)> queue = new();

        foreach (var node in _startNodes)
        {
            queue.Enqueue(new(node, null));
        }

        while (queue.Any())
        {
            var tuple = queue.Dequeue();

            if (tuple.from != null)
            {
                var distance = (tuple.from.Position - tuple.node.Position).Length();

                tuple.node.PathLengths[tuple.from] = distance;
            }

            foreach (var nextNode in tuple.node.GetNextNodes())
            {
                queue.Enqueue(new(nextNode, tuple.node));
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
        var data = JsonSerializer.Serialize(_startNodes);
        var workDir = System.IO.Directory.GetCurrentDirectory();
        File.WriteAllText(workDir + "/Saves/" + filename + ".json", data);
    }
}