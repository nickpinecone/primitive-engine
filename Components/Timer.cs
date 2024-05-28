using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public class Timer : GameObject
{
    public event EventHandler OnTimeout;

    public double WaitTime { get; set; }
    public double CurrentTime { get; private set; }
    public bool OneShot { get; set; }
    public bool Paused { get; set; }
    public bool Done { get; private set; }

    public Timer(GameObject parent, double waitTime, bool paused = true, bool oneShot = true) : base(parent)
    {
        WaitTime = waitTime;
        CurrentTime = 0;
        Paused = paused;
        OneShot = oneShot;
    }

    public void Restart()
    {
        Paused = false;
        Done = false;
        CurrentTime = 0;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (Paused || Done) return;

        CurrentTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (CurrentTime >= WaitTime)
        {
            Done = true;
            OnTimeout?.Invoke(this, null);

            if (!OneShot)
            {
                Restart();
            }
        }
    }
}