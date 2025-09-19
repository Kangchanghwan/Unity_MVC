using UnityEngine;

namespace Feature.Common
{
    public interface IEvent
    {
    }

    public enum EventChain
    {
        Continue,
        Break
    }

    public interface IMonoEventListener
    {
        GameObject gameObject { get; }
        EventChain OnEventHandle(IEvent @event);
    }

    public interface IMonoEventDispatcher
    {
        GameObject gameObject { get; }
    }

    public static class MonoEventDispatcherExtensions
    {
        public static void Emit(this IMonoEventDispatcher dispatcher, IEvent @event)
        {
            if (dispatcher == null || dispatcher.gameObject == null) return;

            var eventListeners = dispatcher.gameObject.GetComponentsInParent<IMonoEventListener>();
            foreach (var listener in eventListeners)
            {
                if (listener.OnEventHandle(@event) == EventChain.Break) return;
            }
        }

        public static void Emit<T>(this IMonoEventDispatcher dispatcher) where T : IEvent, new()
        {
            if (dispatcher == null || dispatcher.gameObject == null) return;
            
            var eventListeners = dispatcher.gameObject.GetComponentsInParent<IMonoEventListener>();
            foreach (var listener in eventListeners)
            {
                if (listener.OnEventHandle(new T()) == EventChain.Break) return;
            }
        }
    }
}