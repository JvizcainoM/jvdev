using UnityEngine;

namespace JV.SpawnSystem
{
    public class EntityFactory<T> : IEntityFactory<T> where T : MonoBehaviour
    {
        private readonly EntityData[] _entityData;
        private int _iterator;

        public EntityFactory(EntityData[] entityData)
        {
            _entityData = entityData;
        }

        public T Create(Transform spawnPoint)
        {
            var entityData = _entityData[_iterator++ % _entityData.Length];
            var entity = Object.Instantiate(entityData.prefab, spawnPoint.position, spawnPoint.rotation);
            return entity.GetComponent<T>();
        }
    }
}