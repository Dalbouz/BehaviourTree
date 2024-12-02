using System.Collections.Generic;
using UnityEngine;
using System;

namespace KrampStudio.Events
{
    /// <summary>
    /// Event system calss for adding, removing and calling the events.
    /// </summary>
    public class DEventSystem : MonoBehaviour
    {
        private static readonly Dictionary<Type, Delegate> _eventListeners = new Dictionary<Type, Delegate>();

        public delegate void EventDelegate<T>(T e) where T : EventRoot;

        /// <summary>
        /// Add an event delegate to the event listener list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="del"></param>
        public static void AddListener<T>(EventDelegate<T> del) where T : EventRoot
        {
            var eventType = typeof(T);
            if (_eventListeners.ContainsKey(eventType))
            {
                _eventListeners[eventType] = Delegate.Combine(_eventListeners[eventType], del);
            }
            else
            {
                _eventListeners[eventType] = del;
            }
        }

        /// <summary>
        /// Remove a specific delegate from the event listener list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="del"></param>
        public static void RemoveListener<T>(EventDelegate<T> del) where T : EventRoot
        {
            var eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var currentDel))
            {
                var newDel = Delegate.Remove(currentDel, del);
                if (newDel == null)
                {
                    _eventListeners.Remove(eventType);
                }
                else
                {
                    _eventListeners[eventType] = newDel;
                }
            }
        }

        /// <summary>
        /// Call all specific delegates that are subscribed to the <paramref name="eventRoot"/> event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventRoot"></param>
        public static void Raise<T>(T eventRoot) where T : EventRoot
        {
            var eventType = typeof(T);
            if (_eventListeners.TryGetValue(eventType, out var del))
            {
                var eventDelegate = del as EventDelegate<T>;
                if (eventDelegate != null)
                {
                    eventDelegate.Invoke(eventRoot);
                }
            }
        }
    }
}
