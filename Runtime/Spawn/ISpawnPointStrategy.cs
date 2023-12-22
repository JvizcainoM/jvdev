using UnityEngine;

namespace Workshop.SpawnSystem
{
    public interface ISpawnPointStrategy
    {
        Transform NextSpawnPoint();
    }
}