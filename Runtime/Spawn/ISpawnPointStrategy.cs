using UnityEngine;

namespace JvDev.Spawn
{
    public interface ISpawnPointStrategy
    {
        Transform NextSpawnPoint();
    }
}