using System;
using System.Collections.Generic;
using UnifyMe.Core.Enums;

namespace UnifyMe.Core.Events
{
    public class EventsManager
    {
        public static EventsManager Instance;
        private IDictionary<EventsNameEnums, IList<Action>> subscribers;

        public EventsManager()
        {
            this.subscribers = new Dictionary<EventsNameEnums, IList<Action>>();
            Instance = this;
        }

        public void SubScribe(EventsNameEnums eventName, Action callback)
        {
            if (!this.subscribers.ContainsKey(eventName))
            {
                this.subscribers.Add(eventName, new List<Action>() { callback });
                return;
            }

            if (!this.subscribers[eventName].Contains(callback))
                this.subscribers[eventName].Add(callback);
        }

        public void Raise(EventsNameEnums eventName, EventArgs parameters)
        {
            if (this.subscribers.ContainsKey(eventName))
                foreach (Action callback in this.subscribers[eventName])
                    try
                    {
                        if (parameters == EventArgs.Empty)
                            callback.DynamicInvoke();
                        else
                            callback.DynamicInvoke(parameters);
                    }
                    catch (Exception e)
                    {
                        //EMPTY
                    }
        }

        public void UnSubScribe(EventsNameEnums eventName, Action callback)
        {
            if (this.subscribers.ContainsKey(eventName))
                this.subscribers[eventName].Remove(callback);
        }
    }
}
