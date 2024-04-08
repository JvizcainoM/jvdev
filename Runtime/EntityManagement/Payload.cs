using UnityEngine;

namespace JvDev.Mediator
{
    public abstract class Payload<TData> : IVisitor
    {
        public abstract TData Content { get; set; }
        public abstract void Visit<T>(T entity) where T : Component, IVisitable;
    }
}