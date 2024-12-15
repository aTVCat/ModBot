﻿using System;
using System.Collections.Generic;

namespace ModLibrary
{
    /// <summary>
    /// Defines extension methods for the <see cref="GlobalEventManager"/> class
    /// </summary>
    public static class GlobalEventManagerExtensions
    {
        /// <summary>
        /// Adds an event that can be dispatched only once if it's not already defined
        /// </summary>
        /// <param name="globalEventManager"></param>
        /// <param name="eventName">The name of the event to dispatch from, see <see cref="GlobalEvents"/> for a complete list</param>
        /// <param name="callback">The <see cref="Action"/> to invoke when the event is dispatched</param>
        public static void TryAddEventListenerOnce(this GlobalEventManager globalEventManager, string eventName, Action callback)
        {
            List<object> onceCallbackList = globalEventManager.getOnceListenersFor(eventName);

            if (!onceCallbackList.Contains(callback))
                onceCallbackList.Add(callback);
        }

        /// <summary>
        /// Adds an event with an argument that can be dispatched only once if it's not already defined
        /// </summary>
        /// <typeparam name="T">The type of the argument to pass to the <see cref="Action{T}"/> when the event is dispatched</typeparam>
        /// <param name="globalEventManager"></param>
        /// <param name="eventName">The name of the event to dispatch from, see <see cref="GlobalEvents"/> for a complete list</param>
        /// <param name="callback">The <see cref="Action{T}"/> to invoke when the event is dispatched</param>
        public static void TryAddEventListenerOnce<T>(this GlobalEventManager globalEventManager, string eventName, Action<T> callback)
        {
            List<object> onceCallbackList = globalEventManager.getOnceListenersFor(eventName);

            if (!onceCallbackList.Contains(callback))
                onceCallbackList.Add(callback);
        }

        /// <summary>
        /// Adds an event with an argument that can be dispatched if it's not already defined
        /// </summary>
        /// <typeparam name="T">The type of the argument to pass to the <see cref="Action{T}"/> when the event is dispatched</typeparam>
        /// <param name="globalEventManager"></param>
        /// <param name="eventName">The name of the event to dispatch from, see <see cref="GlobalEvents"/> for a complete list</param>
        /// <param name="callback">The <see cref="Action{T}"/> to invoke when the event is dispatched</param>
        public static void TryAddEventListener<T>(this GlobalEventManager globalEventManager, string eventName, Action<T> callback)
        {
            List<object> callbackList = globalEventManager.getEventListenersFor(eventName);

            if (!callbackList.Contains(callback))
                callbackList.Add(callback);
        }

        /// <summary>
        /// Adds an event that can be dispatched if it's not already defined
        /// </summary>
        /// <param name="globalEventManager"></param>
        /// <param name="eventName">The name of the event to dispatch from, see <see cref="GlobalEvents"/> for a complete list</param>
        /// <param name="callback">The <see cref="Action"/> to invoke when the event is dispatched</param>
        public static void TryAddEventListener(this GlobalEventManager globalEventManager, string eventName, Action callback)
        {
            List<object> callbackList = globalEventManager.getEventListenersFor(eventName);

            if (!callbackList.Contains(callback))
                callbackList.Add(callback);
        }
    }
}
