using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

abstract public class Component
{
    public GameObject Parent { get; set; }

    public Component(GameObject parent)
    {
        Parent = parent;
    }

    abstract public void HandleInput();
    abstract public void Update(GameTime gameTime);
    abstract public void Draw(SpriteBatch spriteBatch);
}