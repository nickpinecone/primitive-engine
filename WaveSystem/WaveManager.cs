using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TowerDefense;

public class EnemyInfo
{
    public string TypeName { get; set; }
    public int Amount { get; set; }
    public int Order { get; set; }

    public EnemyInfo(string typeName, int order, int amount)
    {
        TypeName = typeName;
        Order = order;
        Amount = amount;
    }
}

public class WaveInfo
{
    public int WaveNumber { get; set; }
    public Dictionary<string, EnemyInfo> Enemies { get; set; }

    public WaveInfo(int waveNumber)
    {
        WaveNumber = waveNumber;
        Enemies = new();
    }
}

public class NodeWaveInfo
{
    public int StartId { get; set; }
    public Dictionary<int, WaveInfo> Waves { get; set; }

    public NodeWaveInfo(int startId)
    {
        StartId = startId;
        Waves = new();
    }
}

public class WaveManager
{
    private WalkPath _walkPath;
    private Dictionary<int, NodeWaveInfo> _nodeWaves { get; set; }

    public WaveManager(WalkPath walkPath)
    {
        _walkPath = walkPath;

        _nodeWaves = new();
    }

    public void SaveToFile(string filename)
    {
        MetaManager.SaveToFile(_nodeWaves, filename);
    }

    public void LoadFromFile(string filename)
    {
        _nodeWaves = MetaManager.ReadFromFile<Dictionary<int, NodeWaveInfo>>(filename);
    }

    public EnemyInfo GetEnemyInfo(int startId, int waveNumber, string typeName)
    {
        if (!_nodeWaves.ContainsKey(startId))
        {
            _nodeWaves[startId] = new(startId);
        }

        if (!_nodeWaves[startId].Waves.ContainsKey(waveNumber))
        {
            _nodeWaves[startId].Waves[waveNumber] = new(waveNumber);
        }

        if (!_nodeWaves[startId].Waves[waveNumber].Enemies.ContainsKey(typeName))
        {
            _nodeWaves[startId].Waves[waveNumber].Enemies[typeName] = new(typeName, 0, 0);
        }

        return _nodeWaves[startId].Waves[waveNumber].Enemies[typeName];
    }

    public void StoreEnemyInfo(int startId, int waveNumber, string typeName, int order, int amount)
    {
        if (!_nodeWaves.ContainsKey(startId))
        {
            _nodeWaves[startId] = new(startId);
        }

        if (!_nodeWaves[startId].Waves.ContainsKey(waveNumber))
        {
            _nodeWaves[startId].Waves[waveNumber] = new(waveNumber);
        }

        if (!_nodeWaves[startId].Waves[waveNumber].Enemies.ContainsKey(typeName))
        {
            _nodeWaves[startId].Waves[waveNumber].Enemies[typeName] = new(typeName, 0, 0);
        }

        _nodeWaves[startId].Waves[waveNumber].Enemies[typeName] = new(typeName, order, amount);
    }
}