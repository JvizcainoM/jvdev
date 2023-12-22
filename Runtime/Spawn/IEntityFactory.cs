using UnityEngine;

namespace Workshop
{
    public interface IEntityFactory<out T> where T : MonoBehaviour
    {
        T Create(Transform spawnPoint);
    }
}