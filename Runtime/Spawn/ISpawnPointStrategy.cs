using UnityEngine;

namespace SpawnSystem
{
    public interface ISpawnPointStrategy
    {
        Transform NextSpawnPoint();
    }
}