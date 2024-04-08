using UnityEngine;

namespace JvDev
{
    public interface IVisitor
    {
        void Visit<T>(T entity) where T : Component, IVisitable;
    }
}