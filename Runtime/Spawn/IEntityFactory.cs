using UnityEngine;

namespace JV.SpawnSystem
{
    public interface IEntityFactory<out T> where T : MonoBehaviour
    {
        T Create(Transform spawnPoint);
    }
}