using System;

namespace JvDev.EventBus
{
    public class EventBinding<T> : IEventBinding<T> where T : IEvent
    {
        public EventBinding(Action<T> onEvent) => OnEvent = onEvent;
        public EventBinding(Action onEventNoArgs) => OnEventNoArgs = onEventNoArgs;

        public Action<T> OnEvent { get; set; } = _ => { };
        public Action OnEventNoArgs { get; set; } = () => { };

        public void Add(Action<T> onEvent) => OnEvent += onEvent;
        public void Add(Action onEventNoArgs) => OnEventNoArgs += onEventNoArgs;
        public void Remove(Action<T> onEvent) => OnEvent -= onEvent;
        public void Remove(Action onEventNoArgs) => OnEventNoArgs -= onEventNoArgs;
    }
}