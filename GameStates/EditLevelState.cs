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
            var nodeType = NodeType.Regular;

            if (keyState.IsKeyDown(Keys.S))
            {
                nodeType = NodeType.Start;
                walkPath.AddStartNode(node);
            }
            else if (keyState.IsKeyDown(Keys.E))
            {
                nodeType = NodeType.End;
            }

            var pathNode = new PathNode(node, NodeSize, nodeType);
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
        Dictionary<Node, PathNode> dict = new();

        foreach (var tuple in walkPath.Enumerate())
        {
            PathNode pathNode;

            if (dict.ContainsKey(tuple.node))
            {
                pathNode = dict[tuple.node];
            }
            else
            {
                pathNode = new PathNode(tuple.node, NodeSize, tuple.node.nodeType);
                dict[tuple.node] = pathNode;
                AddGameObject(pathNode);
            }

            if (tuple.from != null)
            {
                dict[tuple.from].LinkNode(pathNode);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}