using UnityEngine;

namespace SpawnSystem
{
    public class LinearSpawnPointStrategy : ISpawnPointStrategy
    {
        private readonly Transform[] _spawnPoints;
        private int _currentIndex;

        public LinearSpawnPointStrategy(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }

        public Transform NextSpawnPoint()
        {
            var spawnPoint = _spawnPoints[_currentIndex];
            _currentIndex = (_currentIndex + 1) % _spawnPoints.Length;
            return spawnPoint;
        }
    }
}