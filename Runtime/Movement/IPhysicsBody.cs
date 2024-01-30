using UnityEngine;

namespace JvDev.Movement
{
    public interface IPhysicsBody
    {
        bool IsActive { get; set; }
        void Step();
        Vector3 Velocity { get; }
        void AddForce(Vector3 force, ForceMode mode = 0);
    }
}