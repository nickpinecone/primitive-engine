using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense;

public static class AssetManager
{
    private static ContentManager _content = null;
    private static Dictionary<string, object> _assets = new();

    public static void Initialize(ContentManager contentManager)
    {
        _content = contentManager;
    }

    public static T GetAsset<T>(string name)
    {
        if (!_assets.ContainsKey(name))
        {
            _assets[name] = _content.Load<T>(name);
        }

        return (T)_assets[name];
    }

    public static void UnloadAssets()
    {
        _content.Unload();
        _assets.Clear();
    }
}