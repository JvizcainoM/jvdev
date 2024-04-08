using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JvDev.Mediator
{
    public abstract class Mediator<T> : MonoBehaviour where T : Component, IVisitable
    {
        protected readonly List<T> Entities = new();

        public void Register(T entity)
        {
            if (Entities.Contains(entity)) return;
            Entities.Add(entity);
            OnRegister(entity);
        }

        public void Remove(T entity)
        {
            if (!Entities.Contains(entity)) return;
            Entities.Remove(entity);
            OnRemove(entity);
        }

        public void Message(T source, T target, IVisitor message)
        {
            Entities.FirstOrDefault(e => e == target)?.Accept(message);
        }

        public void Broadcast(T source, IVisitor message, Func<T, bool> predicate = null)
        {
            Entities
                .Where(target => source != target
                                 && SenderConditionMet(target, predicate)
                                 && MediatorConditionMet(target))
                .ToList().ForEach(target => target.Accept(message));
        }

        private static bool SenderConditionMet(T target, Func<T, bool> predicate)
        {
            return predicate == null || predicate(target);
        }

        protected virtual bool MediatorConditionMet(T entity) => true;

        protected virtual void OnRegister(T entity)
        {
        }

        protected virtual void OnRemove(T entity)
        {
        }
    }
}