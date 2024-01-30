using UnityEngine;

namespace JvDev.Spawn
{
    public class EntitySpawner<T> where T : MonoBehaviour
    {
        private readonly IEntityFactory<T> _entityFactory;
        private readonly ISpawnPointStrategy _spawnPointStrategy;

        public EntitySpawner(IEntityFactory<T> entityFactory, ISpawnPointStrategy spawnPointStrategy)
        {
            _entityFactory = entityFactory;
            _spawnPointStrategy = spawnPointStrategy;
        }

        public T Spawn()
        {
            var spawnTransform = _spawnPointStrategy.NextSpawnPoint();
            return _entityFactory.Create(spawnTransform);
        }
    }
}