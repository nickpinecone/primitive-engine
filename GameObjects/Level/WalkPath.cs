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

public enum NodeType { Start, End, Regular };

public class Node
{
    public event EventHandler OnStartRemove;

    private static int StaticStartId = 0;

    private List<Node> _nextNodes;

    private int _startId;
    public int StartId
    {
        get { return _startId; }
        set
        {
            _startId = value;
            if (value > StaticStartId)
            {
                StaticStartId = value;
            }
        }
    }

    public Vector2 Position { get; set; }
    public Dictionary<Node, double> PathLengths { get; set; }

    private NodeType _type = NodeType.Regular;
    public NodeType Type
    {
        get { return _type; }
        set
        {
            _type = value;
            if (value == NodeType.Start && StartId <= 0)
            {
                StaticStartId += 1;
                StartId = StaticStartId;
            }
        }
    }

    public Node(Vector2 position)
    {
        Position = position;

        _nextNodes = new();
        PathLengths = new();
    }

    public List<Node> GetNextNodes()
    {
        return _nextNodes;
    }

    public Node PickNextNode()
    {
        if (_nextNodes.Count > 0)
        {
            var index = RandomGenerator.Rng.Next(_nextNodes.Count);

            return _nextNodes[index];
        }
        else
        {
            return null;
        }
    }

    public void LinkNode(Node otherNode)
    {
        _nextNodes.Add(otherNode);
    }

    public void UnlinkNode(Node node)
    {
        _nextNodes.Remove(node);
    }

    public void RemoveStart()
    {
        OnStartRemove?.Invoke(this, null);
    }
}

public class WalkPath
{
    private Dictionary<Enemy, Node> _enemyNodes;
    private List<Node> _startNodes;

    public WalkPath()
    {
        _enemyNodes = new();
        _startNodes = new();
    }

    public void Initialize(string filename)
    {
        MetaManager.LoadWalkPath(filename, this);
        CalculateLengths();
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

    public Node GetStartById(int startId)
    {
        foreach (var node in _startNodes)
        {
            if (node.StartId == startId)
            {
                return node;
            }
        }

        return null;
    }

    public void AddStartNode(Node node)
    {
        node.OnStartRemove += (obj, _) => RemoveStartNode((Node)obj);

        _startNodes.Add(node);
    }

    public void RemoveStartNode(Node node)
    {
        _startNodes.Remove(node);
    }

    public List<Node> GetNodesInRadius(Vector2 position, int radius)
    {
        var nodes = new List<Node>();

        foreach (var tuple in Enumerate())
        {
            if (Vector2.Distance(position, tuple.node.Position) <= radius)
            {
                nodes.Add(tuple.node);
            }
        }

        return nodes;
    }

    public List<Enemy> GetEnemiesToPoints(List<Node> points)
    {
        var enemies = new List<Enemy>();

        foreach (var (enemy, node) in _enemyNodes)
        {
            if (enemy.Dead)
            {
                _enemyNodes.Remove(enemy);
            }
            else if (points.Contains(node))
            {
                enemies.Add(enemy);
            }
        }

        return enemies;
    }

    public Node GetNextPoint(Enemy enemy)
    {
        if (!_enemyNodes.ContainsKey(enemy))
        {
            _enemyNodes[enemy] = enemy.FromNode.PickNextNode();
        }

        var toNode = _enemyNodes[enemy];
        if (toNode == null) return null;

        if (enemy.MovedDistance >= toNode.PathLengths[enemy.FromNode])
        {
            enemy.MovedDistance = 0;
            enemy.FromNode = toNode;
            var nextNode = toNode.PickNextNode();
            _enemyNodes[enemy] = nextNode;
            toNode = nextNode;
        }

        return toNode;
    }
}