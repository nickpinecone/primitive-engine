using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using TowerDefense;

public class LevelInfo
{
    public string EnemySave { get; private set; }
    public string LevelSave { get; private set; }
    public string WalkPathSave { get; private set; }

    public LevelInfo(int count)
    {
        EnemySave = $"enemy{count}";
        LevelSave = $"level{count}";
        WalkPathSave = $"walkpath{count}";
    }
}

class LevelPoint : GameObject
{
    static public int LevelCount = 0;

    public event EventHandler OnLevelSelect;

    public LevelInfo LevelInfo { get; }
    public Sprite Sprite { get; }
    public CollisionShape Shape { get; }
    public Interact Interact { get; }

    public LevelPoint(GameObject parent, Vector2 position, float scale) : base(parent)
    {
        LevelInfo = new LevelInfo(LevelCount);

        var texture = AssetManager.GetAsset<Texture2D>("Sprites/LevelSheet");
        var source = new Rectangle(1120, 675, 75, 150);

        Sprite = new Sprite(this, texture, source, 2);
        Shape = new CollisionShape(this, Sprite.Size);
        Interact = new Interact(this, Sprite, Shape);

        Interact.OnDoubleSelect += HandleSelection;

        LocalPosition = position;
        LocalScale = scale;
    }

    protected override void QueueFree()
    {
        // TODO Delete associated files

        base.QueueFree();
    }

    public void HandleSelection(object sender, EventArgs args)
    {
        OnLevelSelect?.Invoke(this, null);
    }
}