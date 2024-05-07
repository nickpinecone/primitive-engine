using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

class Node
{
    private static int IDCount = -1;

    private List<Node> _prevNodes;
    private List<Node> _nextNodes;

    public int ID { get; protected set; }
    public Vector2 Position { get; set; }
    public Dictionary<Node, double> PathLengths { get; set; }

    public Node(Vector2 position)
    {
        IDCount += 1;
        ID = IDCount;

        _prevNodes = new();
        _nextNodes = new();
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

    public void DeleteNode()
    {
        foreach (var node in _prevNodes)
        {
            node._nextNodes.Remove(this);
        }
        foreach (var node in _nextNodes)
        {
            node._prevNodes.Remove(this);
        }
    }

    public void LinkNode(Node otherNode)
    {
        this._nextNodes.Add(otherNode);
        otherNode._prevNodes.Add(this);
    }
}

struct OriginNode
{
    public Node node;
    public Node fromNode;

    public OriginNode(Node node, Node fromNode)
    {
        this.node = node;
        this.fromNode = fromNode;
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
        Queue<OriginNode> queue = new();
        foreach (var node in _startNodes)
        {
            queue.Enqueue(new OriginNode(node, null));
        }

        while (queue.Any())
        {
            var origin = queue.Dequeue();

            if (origin.fromNode != null)
            {
                var distance = (origin.fromNode.Position - origin.node.Position).Length();

                origin.node.PathLengths[origin.fromNode] = distance;
            }

            foreach (var nextNode in origin.node.GetNextNodes())
            {
                queue.Enqueue(new OriginNode(nextNode, origin.node));
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
}