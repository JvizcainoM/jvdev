using UnityEngine;

namespace JvDev.Spawn
{
    public interface IEntityFactory<out T> where T : MonoBehaviour
    {
        T Create(Transform spawnPoint);
    }
}