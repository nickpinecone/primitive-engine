using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class EditLevelState : GameState
{
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
            var pathNode = new PathNode(node, 20);
            AddGameObject(pathNode);
            if (keyState.IsKeyDown(Keys.S))
            {
                walkPath.AddStartNode(node);
            }
            walkPath.SaveToFile("");
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
}