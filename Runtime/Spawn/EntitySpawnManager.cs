using System;
using UnityEngine;
using Workshop.SpawnSystem;

namespace Workshop
{
    public abstract class EntitySpawnManager : MonoBehaviour
    {
        [SerializeField] protected StrategyType spawnPointStrategyType = StrategyType.Linear;
        [SerializeField] protected Transform[] spawnPoints;

        protected ISpawnPointStrategy SpawnPointStrategy;
        protected int SpawnPointCount => spawnPoints.Length;

        protected virtual void Awake()
        {
            SpawnPointStrategy = spawnPointStrategyType switch
            {
                StrategyType.Random => new RandomSpawnPointStrategy(spawnPoints),
                StrategyType.Linear => new LinearSpawnPointStrategy(spawnPoints),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public abstract void Spawn();

        protected enum StrategyType
        {
            Random,
            Linear
        }
    }
}