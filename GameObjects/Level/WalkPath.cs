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

    public List<Node> GetNodesInRadius(Vector2 position, float radius)
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
            if (points.Contains(node))
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