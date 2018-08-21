using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace AngryRabbit
{
    /// <summary>
    /// 事件
    /// </summary>
    public class AREvent
    {
        public string EventName { get; set; }

        public string EventDesc { get; set; }

        public EventCheckCondition CheckCondition { get; set; }

        public TimePoint CheckedTime { get; set; }

        public UnityAction EventAction;

        public AREvent(string name, string desc, EventCheckCondition condition, TimePoint time, UnityAction action)
        {
            this.EventName = name;
            this.EventDesc = desc;
            this.CheckCondition = condition;
            this.CheckedTime = time;
            this.EventAction = action;
        }
    }
}
