using UnityEngine;

namespace Workshop.SpawnSystem
{
    public class RandomSpawnPointStrategy : ISpawnPointStrategy
    {
        private readonly Transform[] _spawnPoints;

        public RandomSpawnPointStrategy(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }

        public Transform NextSpawnPoint()
        {
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        }
    }
}