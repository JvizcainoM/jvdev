using System.Collections.Generic;
using UnityEngine;

namespace Workshop
{
    public static class EventBus<T> where T : IEvent
    {
        private static readonly HashSet<IEventBinding<T>> Bindings = new();

        public static void Register(IEventBinding<T> binding) => Bindings.Add(binding);
        public static void UnRegister(IEventBinding<T> binding) => Bindings.Remove(binding);

        public static void Raise(T e)
        {
            foreach (var binding in Bindings)
            {
                binding.OnEvent(e);
                binding.OnEventNoArgs();
            }
        }

        private static void Clear()
        {
            Debug.Log($"Clearing EventBus<{typeof(T).Name}>");
            Bindings.Clear();
        }
    }
}