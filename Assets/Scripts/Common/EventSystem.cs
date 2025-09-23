using System;
using System.Collections.Generic;
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

    // 글로벌 이벤트 리스너 인터페이스
    public interface IEventListener
    {
        EventChain OnEventHandle(IEvent @event);
    }

    // 글로벌 이벤트 매니저
    public static class EventManager
    {
        private static readonly Dictionary<Type, List<IEventListener>> EventListeners = new Dictionary<Type, List<IEventListener>>();
        private static readonly Dictionary<Type, List<Action<IEvent>>> EventActions = new Dictionary<Type, List<Action<IEvent>>>();

        /// <summary>
        /// 이벤트 리스너를 구독합니다.
        /// </summary>
        public static void Subscribe<T>(IEventListener listener) where T : IEvent
        {
            var eventType = typeof(T);
            if (!EventListeners.ContainsKey(eventType))
            {
                EventListeners[eventType] = new List<IEventListener>();
            }
            
            if (!EventListeners[eventType].Contains(listener))
            {
                EventListeners[eventType].Add(listener);
            }
        }

        /// <summary>
        /// 액션 기반 이벤트를 구독합니다.
        /// </summary>
        public static void Subscribe<T>(Action<T> action) where T : IEvent
        {
            var eventType = typeof(T);
            if (!EventActions.ContainsKey(eventType))
            {
                EventActions[eventType] = new List<Action<IEvent>>();
            }
            
            EventActions[eventType].Add(@event => action((T)@event));
        }

        /// <summary>
        /// 이벤트 리스너 구독을 해제합니다.
        /// </summary>
        public static void Unsubscribe<T>(IEventListener listener) where T : IEvent
        {
            var eventType = typeof(T);
            if (EventListeners.ContainsKey(eventType))
            {
                EventListeners[eventType].Remove(listener);
                if (EventListeners[eventType].Count == 0)
                {
                    EventListeners.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 액션 기반 이벤트 구독을 해제합니다.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> action) where T : IEvent
        {
            var eventType = typeof(T);
            if (EventActions.ContainsKey(eventType))
            {
                EventActions[eventType].RemoveAll(a => a.Method == action.Method && a.Target == action.Target);
                if (EventActions[eventType].Count == 0)
                {
                    EventActions.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 특정 리스너의 모든 구독을 해제합니다.
        /// </summary>
        public static void UnsubscribeAll(IEventListener listener)
        {
            foreach (var kvp in EventListeners)
            {
                kvp.Value.Remove(listener);
            }
            
            // 빈 리스트 제거
            var keysToRemove = new List<Type>();
            foreach (var kvp in EventListeners)
            {
                if (kvp.Value.Count == 0)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                EventListeners.Remove(key);
            }
        }

        /// <summary>
        /// 글로벌 이벤트를 발행합니다.
        /// </summary>
        public static void Publish(IEvent @event)
        {
            var eventType = @event.GetType();
            
            // IEventListener 기반 구독자들에게 발행
            if (EventListeners.ContainsKey(eventType))
            {
                var listeners = new List<IEventListener>(EventListeners[eventType]);
                foreach (var listener in listeners)
                {
                    try
                    {
                        if (listener.OnEventHandle(@event) == EventChain.Break)
                            break;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error handling event {eventType.Name}: {ex.Message}");
                    }
                }
            }
            
            // Action 기반 구독자들에게 발행
            if (EventActions.ContainsKey(eventType))
            {
                var actions = new List<Action<IEvent>>(EventActions[eventType]);
                foreach (var action in actions)
                {
                    try
                    {
                        action(@event);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error handling event {eventType.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 새 인스턴스로 글로벌 이벤트를 발행합니다.
        /// </summary>
        public static void Publish<T>() where T : IEvent, new()
        {
            Publish(new T());
        }

        /// <summary>
        /// 모든 구독을 해제합니다. (주로 Scene 전환 시 사용)
        /// </summary>
        public static void Clear()
        {
            EventListeners.Clear();
            EventActions.Clear();
        }

        /// <summary>
        /// 현재 구독 상태를 디버깅용으로 출력합니다.
        /// </summary>
        public static void DebugSubscriptions()
        {
            Debug.Log("=== EventManager Subscriptions ===");
            foreach (var kvp in EventListeners)
            {
                Debug.Log($"Event: {kvp.Key.Name}, Listeners: {kvp.Value.Count}");
            }
            foreach (var kvp in EventActions)
            {
                Debug.Log($"Event: {kvp.Key.Name}, Actions: {kvp.Value.Count}");
            }
        }
    }

    // MonoBehaviour 기반 이벤트 시스템 확장
    public static class MonoEventDispatcherExtensions
    {
        /// <summary>
        /// 상위 컴포넌트들에게 이벤트를 발행합니다. (기존 기능)
        /// </summary>
        public static void Emit(this IMonoEventDispatcher dispatcher, IEvent @event)
        {
            if (dispatcher == null || dispatcher.gameObject == null) return;

            var eventListeners = dispatcher.gameObject.GetComponentsInParent<EventListenerMono>();
            foreach (var listener in eventListeners)
            {
                if (listener.OnEventHandle(@event) == EventChain.Break) return;
            }
        }

        /// <summary>
        /// 상위 컴포넌트들에게 새 인스턴스로 이벤트를 발행합니다. (기존 기능)
        /// </summary>
        public static void Emit<T>(this IMonoEventDispatcher dispatcher) where T : IEvent, new()
        {
            if (dispatcher == null || dispatcher.gameObject == null) return;
            
            var eventListeners = dispatcher.gameObject.GetComponentsInParent<EventListenerMono>();
            foreach (var listener in eventListeners)
            {
                if (listener.OnEventHandle(new T()) == EventChain.Break) return;
            }
        }

        /// <summary>
        /// 글로벌 이벤트 매니저를 통해 이벤트를 발행합니다.
        /// </summary>
        public static void EmitGlobal(this IMonoEventDispatcher dispatcher, IEvent @event)
        {
            EventManager.Publish(@event);
        }

        /// <summary>
        /// 글로벌 이벤트 매니저를 통해 새 인스턴스로 이벤트를 발행합니다.
        /// </summary>
        public static void EmitGlobal<T>(this IMonoEventDispatcher dispatcher) where T : IEvent, new()
        {
            EventManager.Publish<T>();
        }

        /// <summary>
        /// 로컬(상위 컴포넌트)과 글로벌 모두에 이벤트를 발행합니다.
        /// </summary>
        public static void EmitBoth(this IMonoEventDispatcher dispatcher, IEvent @event)
        {
            dispatcher.Emit(@event);
            dispatcher.EmitGlobal(@event);
        }

        /// <summary>
        /// 로컬(상위 컴포넌트)과 글로벌 모두에 새 인스턴스로 이벤트를 발행합니다.
        /// </summary>
        public static void EmitBoth<T>(this IMonoEventDispatcher dispatcher) where T : IEvent, new()
        {
            dispatcher.Emit<T>();
            dispatcher.EmitGlobal<T>();
        }
    }

    // MonoBehaviour 컴포넌트가 EventManager를 쉽게 사용할 수 있도록 도와주는 베이스 클래스
    public abstract class EventListenerMono : MonoBehaviour, IEventListener
    {
        public abstract EventChain OnEventHandle(IEvent @event);

        protected virtual void OnDestroy()
        {
            EventManager.UnsubscribeAll(this);
        }
    }
}