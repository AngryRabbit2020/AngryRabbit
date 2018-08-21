using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace AngryRabbit
{
    public class TimeEntity : AREntity
    {
        public bool TimeSwitch = true;

        private int hour;
        private int minute;

        private int year;
        private int week;

        private int hourLimit = 24;
        private int minuteLimit = 60;

        private int weekLimit = 52;

        private int timeUpdatePer = 15;

        private float timeUpdateTime = 1f;

        private List<AREvent> eventList;

        private List<UnityAction> eventActionList;

        protected override void Init()
        {
            RegisterMsg((int)MsgLib.TimePause, this);
            RegisterMsg((int)MsgLib.TimeStart, this);
            RegisterMsg((int)MsgLib.TimeForward, this);
            RegisterMsg((int)MsgLib.TimeBackward, this);
            RegisterMsg((int)MsgLib.TimeJump, this);
            RegisterMsg((int)MsgLib.AddTimePoint, this);
            RegisterMsg((int)MsgLib.EventActionFinish, this);

            year = EntityMgr.PlayerPrefsEntityIns.Year;
            week = EntityMgr.PlayerPrefsEntityIns.Week;
            hour = EntityMgr.PlayerPrefsEntityIns.Hour;
            minute = EntityMgr.PlayerPrefsEntityIns.Minute;

            eventList = new List<AREvent>();

            eventActionList = new List<UnityAction>();

            UpdateTime();
        }

        public override void HandleMsg(int msg_id, List<object> paramList)
        {
            if (msg_id == (int)MsgLib.TimePause)
            {
                TimeSwitch = false;
            }
            if (msg_id == (int)MsgLib.TimeStart)
            {
                TimeSwitch = true;
            }
            if (msg_id == (int)MsgLib.TimeForward)
            {
                TimeSwitch = true;
                timeCount = timeUpdateTime;
            }
            if (msg_id == (int)MsgLib.TimeBackward)
            {
                TimeSwitch = true;
                timeCount = 0;
                minute -= timeUpdatePer;

                if (minute < 0)
                {
                    hour--;
                    minute = minuteLimit - timeUpdatePer;
                }

                if (hour < 0)
                {
                    week--;
                    hour = hourLimit - 1;
                }

                if (week < 1)
                {
                    year--;
                    week = weekLimit - 1;
                }

                UpdateTime();
            }
            if (msg_id == (int)MsgLib.TimeJump)
            {
                TimeSwitch = true;
                timeCount = 0;

                hour = 0;
                minute = 0;

                week++;
                if (week >= weekLimit)
                {
                    week = 1;
                    year++;
                }

                UpdateTime();
            }
            if (msg_id == (int)MsgLib.AddTimePoint && paramList != null)
            {
                for (int i = 0; i < paramList.Count; i++)
                {
                    AREvent arEvent = (AREvent)paramList[i];
                    AddEvent(arEvent);
                }
            }
            if (msg_id == (int)MsgLib.EventActionFinish)
            {
                NextAction();
            }
        }

        private void OperateTime()
        {
            minute += timeUpdatePer;

            if (minute >= minuteLimit)
            {
                minute = minute % minuteLimit;
                hour++;
            }

            if (hour >= hourLimit)
            {
                hour = hour % hourLimit;
                week++;
            }

            if (week >= weekLimit)
            {
                week = 1;
                year++;
            }

            // Save
            EntityMgr.PlayerPrefsEntityIns.Year = year;
            EntityMgr.PlayerPrefsEntityIns.Week = week;
            EntityMgr.PlayerPrefsEntityIns.Hour = hour;
            EntityMgr.PlayerPrefsEntityIns.Minute = minute;

            UpdateTime();

            CheckEvent();
        }

        private void AddEvent(AREvent arEvent)
        {
            Debug.Log(string.Format("事件名称 : {0}, 事件介绍 :{1}, 事件检查类型 : {2}, 事件时间点 : {3} - {4} - {5} - {6}", arEvent.EventName, arEvent.EventDesc, arEvent.CheckCondition, arEvent.CheckedTime.Year, arEvent.CheckedTime.Week, arEvent.CheckedTime.Hour, arEvent.CheckedTime.Minute));
            eventList.Add(arEvent);
        }

        private void CheckEvent()
        {
            for (int i = 0; i < eventList.Count; i++)
            {
                switch (eventList[i].CheckCondition)
                {
                    case EventCheckCondition.EachWeek:
                        if (eventList[i].CheckedTime.Hour == hour && eventList[i].CheckedTime.Minute == minute)
                        {
                            TimeSwitch = false;
                            if (eventList[i].EventAction != null)
                                AddAction(eventList[i].EventAction);
                        }
                        break;
                    case EventCheckCondition.EachYear:
                        if (eventList[i].CheckedTime.Week == week && eventList[i].CheckedTime.Hour == hour && eventList[i].CheckedTime.Minute == minute)
                        {
                            TimeSwitch = false;
                            if (eventList[i].EventAction != null)
                                AddAction(eventList[i].EventAction);
                        }
                        break;
                    case EventCheckCondition.AppointTime:
                        if (eventList[i].CheckedTime.Year == year && eventList[i].CheckedTime.Week == week && eventList[i].CheckedTime.Hour == hour && eventList[i].CheckedTime.Minute == minute)
                        {
                            TimeSwitch = false;
                            if (eventList[i].EventAction != null)
                                AddAction(eventList[i].EventAction);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void AddAction(UnityAction action)
        {
            eventActionList.Add(action);
            if (eventActionList.Count == 1)
            {
                UnityAction ac = eventActionList[0];
                ac();
            }
        }

        private void NextAction()
        {
            eventActionList.RemoveAt(0);
            if (eventActionList.Count > 0)
            {
                UnityAction ac = eventActionList[0];
                ac();
            }
            else
            {
                TimeSwitch = true;
            }
        }

        private void UpdateTime()
        {
            TestButtons.Instance.DateText.text = string.Format("{0}年  第{1}周", year, week);
            TestButtons.Instance.TimeText.text = string.Format("{0}    {1}时  {2}分", hour < 12 ? "上午" : "下午", GetFixedTime(hour), GetFixedTime(minute));
        }

        private string GetFixedTime(int time)
        {
            return time >= 10 ? time.ToString() : "0" + time.ToString();
        }

        float timeCount = 0f;
        public override void update()
        {
            if (TimeSwitch)
                timeCount += Time.deltaTime;

            if (timeCount >= timeUpdateTime)
            {
                OperateTime();

                timeCount = 0f;
            }
        }
    }
}
