using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class EditLevelState : GameState
{
    const int NodeSize = 20;

    WalkPath walkPath = new();

    public override void LoadContent(ContentManager contentManager)
    {
    }

    public override void UnloadContent(ContentManager contentManager)
    {
        AssetManager.UnloadAssets();
    }

    public override void HandleInput()
    {
        var keyState = Keyboard.GetState();
        var mouseState = Mouse.GetState();

        if (Input.IsKeyJustPressed(Keys.Escape))
        {
            SwitchState(new WorldMapState());
        }

        if (Input.IsMouseJustPressed(MouseButton.Middle))
        {
            var node = new Node(mouseState.Position.ToVector2());
            var pathNode = new PathNode(node, NodeSize);

            if (keyState.IsKeyDown(Keys.S))
            {
                walkPath.AddStartNode(node);
                pathNode.IsStart = true;
            }

            AddGameObject(pathNode);
        }

        if (Input.IsKeyJustPressed(Keys.W))
        {
            walkPath.SaveToFile("walk_path");
        }
        if (Input.IsKeyJustPressed(Keys.R))
        {
            walkPath.LoadFromFile("walk_path");
            GeneratePathNodes();
        }

        base.HandleInput();
    }

    public void GeneratePathNodes()
    {
        var startNodes = walkPath.GetStartNodes();
        Queue<(Node node, PathNode from)> queue = new();
        Dictionary<Node, PathNode> dict = new();

        foreach (var node in startNodes)
        {
            queue.Enqueue(new(node, null));
        }

        while (queue.Any())
        {
            var tuple = queue.Dequeue();

            PathNode pathNode;

            if (dict.ContainsKey(tuple.node))
            {
                pathNode = dict[tuple.node];
            }
            else
            {
                pathNode = new PathNode(tuple.node, NodeSize);
                dict[tuple.node] = pathNode;
                AddGameObject(pathNode);
            }

            if (tuple.from != null)
            {
                tuple.from.LinkNode(pathNode);
            }
            else
            {
                pathNode.IsStart = true;
            }

            foreach (var nextNode in tuple.node.GetNextNodes())
            {
                queue.Enqueue(new(nextNode, pathNode));
            }
        }

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}