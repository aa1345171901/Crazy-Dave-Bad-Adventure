using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownPlate
{
    [ExecuteAlways]
    public class EventManager
    {
        private static Dictionary<Type, List<EventListenerBase>> listenerDicts;

        static EventManager()
        {
            listenerDicts = new Dictionary<Type, List<EventListenerBase>>();
        }

        public static void AddListener<EventType>(EventListener<EventType> listener) where EventType : struct
        {
            Type eventType = typeof(EventType);

            if (!listenerDicts.ContainsKey(eventType))
            {
                listenerDicts[eventType] = new List<EventListenerBase>();
            }

            if (!SubscriptionExists(eventType, listener))
            {
                listenerDicts[eventType].Add(listener);
            }
        }

        private static bool SubscriptionExists(Type type, EventListenerBase receiver)
        {
            List<EventListenerBase> receivers;

            if (!listenerDicts.TryGetValue(type, out receivers)) return false;

            bool exists = false;

            for (int i = receivers.Count - 1; i >= 0; i--)
            {
                if (receivers[i] == receiver)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        public static void RemoveListener<EventType>(EventListener<EventType> listener) where EventType : struct
        {
            Type eventType = typeof(EventType);

            if (!listenerDicts.ContainsKey(eventType))
            {
                return;
            }

            List<EventListenerBase> subscriberList = listenerDicts[eventType];

            for (int i = subscriberList.Count - 1; i >= 0; i--)
            {
                if (subscriberList[i] == listener)
                {
                    subscriberList.Remove(subscriberList[i]);

                    if (subscriberList.Count == 0)
                    {
                        listenerDicts.Remove(eventType);
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// 事件触发，通过type调用List中的所有监听者
        /// </summary>
        /// <param name="newEvent">触发的Event类型</param>
        public static void TriggerEvent<EventType>(EventType newEvent) where EventType : struct
        {
            List<EventListenerBase> list;
            if (listenerDicts.TryGetValue(typeof(EventType), out list))
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    (list[i] as EventListener<EventType>).OnEvent(newEvent);
                }
            }
        }
    }

    /// <summary>
    /// Listenner的扩展
    /// </summary>
    public static class EventRegister
    {
        public static void EventStartListening<EventType>(this EventListener<EventType> caller) where EventType : struct
        {
            EventManager.AddListener<EventType>(caller);
        }

        public static void EventStopListening<EventType>(this EventListener<EventType> caller) where EventType : struct
        {
            EventManager.RemoveListener<EventType>(caller);
        }
    }

    public interface EventListenerBase { };

    /// <summary>
    /// 继承该接口的监听者触发事件时调用OnEvent
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface EventListener<T> : EventListenerBase
    {
        void OnEvent(T eventType);
    }
}