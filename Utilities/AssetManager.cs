using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TowerDefense;

public static class AssetManager
{
    private static ContentManager _content = null;
    private static Dictionary<string, object> _assets = new();
    private static Dictionary<string, object> _staticAssets = new();

    public static void Initialize(ContentManager contentManager)
    {
        _content = contentManager;
    }

    public static T GetAsset<T>(string name, bool isStatic = false)
    {
        if (isStatic)
        {
            if (!_staticAssets.ContainsKey(name))
            {
                _staticAssets[name] = _content.Load<T>(name);
            }

            return (T)_staticAssets[name];
        }
        else
        {
            if (!_assets.ContainsKey(name))
            {
                _assets[name] = _content.Load<T>(name);
            }

            return (T)_assets[name];
        }
    }

    public static void UnloadAssets()
    {
        foreach (var asset in _assets)
        {
            if (!_staticAssets.ContainsKey(asset.Key))
            {
                _content.UnloadAsset(asset.Key);
            }
        }

        _assets.Clear();
    }
}