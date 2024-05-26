using System;
using System.Collections.Generic;

public class EnemyInfo
{
    public Type Type { get; set; }
    public int Amount { get; set; }
    public int Order { get; set; }

    public EnemyInfo(Type type, int order, int amount)
    {
        Type = type;
        Order = order;
        Amount = amount;
    }
}

public class WaveInfo
{
    public int WaveNumber { get; set; }
    public Dictionary<Type, EnemyInfo> Enemies { get; set; }

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
    private Dictionary<int, NodeWaveInfo> _nodeWaves { get; }

    public WaveManager(WalkPath walkPath)
    {
        _walkPath = walkPath;

        _nodeWaves = new();
    }

    public EnemyInfo GetEnemyInfo(int startId, int waveNumber, Type type)
    {
        if (!_nodeWaves.ContainsKey(startId))
        {
            _nodeWaves[startId] = new(startId);
        }

        if (!_nodeWaves[startId].Waves.ContainsKey(waveNumber))
        {
            _nodeWaves[startId].Waves[waveNumber] = new(waveNumber);
        }

        if (!_nodeWaves[startId].Waves[waveNumber].Enemies.ContainsKey(type))
        {
            _nodeWaves[startId].Waves[waveNumber].Enemies[type] = new(type, 0, 0);
        }

        return _nodeWaves[startId].Waves[waveNumber].Enemies[type];
    }

    public void StoreEnemyInfo(int startId, int waveNumber, Type type, int order, int amount)
    {
        if (!_nodeWaves.ContainsKey(startId))
        {
            _nodeWaves[startId] = new(startId);
        }

        if (!_nodeWaves[startId].Waves.ContainsKey(waveNumber))
        {
            _nodeWaves[startId].Waves[waveNumber] = new(waveNumber);
        }

        if (!_nodeWaves[startId].Waves[waveNumber].Enemies.ContainsKey(type))
        {
            _nodeWaves[startId].Waves[waveNumber].Enemies[type] = new(type, 0, 0);
        }

        _nodeWaves[startId].Waves[waveNumber].Enemies[type] = new(type, order, amount);
    }
}