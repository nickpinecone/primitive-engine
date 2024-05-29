using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public List<EnemyInfo> GetByOrder(int order)
    {
        return Enemies.Values.Where((enemy) => enemy.Order == order).ToList();
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

public class WaveManager : GameObject
{
    private WalkPath _walkPath;
    private Dictionary<int, NodeWaveInfo> _nodeWaves;
    private Dictionary<int, Queue<EnemyInfo>> _enemies;

    public int MaxWave { get; private set; }
    public int CurrentWave { get; private set; }
    public int CurrentOrder { get; private set; }

    public Dictionary<int, NodeWaveInfo> NodeWaves { get { return _nodeWaves; } }

    public Timer WaveTimer { get; }
    public Timer OrderTimer { get; }
    public Timer AmountTimer { get; }

    public WaveManager(GameObject parent, WalkPath walkPath) : base(parent)
    {
        _walkPath = walkPath;
        _nodeWaves = new();
        _enemies = new();

        WaveTimer = new Timer(this, 10);
        WaveTimer.OnTimeout += HandleWaveTimeout;

        OrderTimer = new Timer(this, 5);
        OrderTimer.OnTimeout += HandleOrderTimeout;

        AmountTimer = new Timer(this, 2.5);
        AmountTimer.OnTimeout += HandleAmountTimeout;

        CurrentWave = -1;
    }

    public void Initialize()
    {
        _nodeWaves = MetaManager.LoadWaveManager("enemy_editor");

        if (_nodeWaves == null)
        {
            _nodeWaves = new();
            return;
        }

        // Integriy check
        foreach (var nodeId in _nodeWaves.Keys)
        {
            var node = _walkPath.GetStartById(nodeId);

            if (node == null)
            {
                _nodeWaves.Remove(nodeId);
            }
        }

        SetMaxWave();
    }

    public void Start()
    {
        WaveTimer.Restart();
    }

    private void SetMaxWave()
    {
        if (_nodeWaves.Count > 0)
        {
            MaxWave =
                _nodeWaves.Values
                .SelectMany((nodeWaveInfo) => nodeWaveInfo.Waves.Values)
                .Max((waveInfo) => waveInfo.WaveNumber);
        }
    }

    private void SpawnEnemy(EnemyInfo enemyInfo, int startId)
    {
        var node = _walkPath.GetStartById(startId);

        var basicOrk = new BasicOrk(null, _walkPath, node, 0.5f);

        SpawnObject(basicOrk);
    }

    private void HandleAmountTimeout(object sender, EventArgs args)
    {
        foreach (var key in _nodeWaves.Keys)
        {
            var queue = _enemies[key];
            if (queue.Count > 0)
            {
                var enemyInfo = queue.Dequeue();

                // Create enemy
                SpawnEnemy(enemyInfo, key);
            }
        }

        if (_enemies.Values.All((queue) => queue.Count <= 0))
        {
            // Done with current order
            CurrentOrder += 1;
            OrderTimer.Restart();
        }
        else
        {
            AmountTimer.Restart();
        }
    }

    private void HandleOrderTimeout(object sender, EventArgs args)
    {
        foreach (var (key, value) in _nodeWaves)
        {
            foreach (var enemyInfo in _nodeWaves[key].Waves[CurrentWave].GetByOrder(CurrentOrder))
            {
                if (!_enemies.ContainsKey(key))
                {
                    _enemies[key] = new();
                }

                for (int i = 0; i < enemyInfo.Amount; i++)
                {
                    _enemies[key].Enqueue(enemyInfo);
                }
            }
        }

        if (_enemies.Values.All((queue) => queue.Count <= 0))
        {
            // Done with current wave
            WaveTimer.Restart();
        }
        else
        {
            AmountTimer.Restart();
        }
    }

    private void HandleWaveTimeout(object sender, EventArgs args)
    {
        CurrentWave += 1;
        if (CurrentWave > MaxWave)
        {
            // Completed level
        }
        else
        {
            OrderTimer.Restart();
            CurrentOrder = 0;
        }
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