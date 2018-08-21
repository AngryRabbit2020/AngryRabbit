using UnityEngine;
using System.Collections;

namespace AngryRabbit
{
    public class CommonDefines
    {
         
    }

    public struct TimePoint
    {
        public int Year;
        public int Week;
        public int Hour;
        public int Minute;

        public TimePoint(int year, int week, int hour, int minute)
        {
            this.Year = year;
            this.Week = week;
            this.Hour = hour;
            this.Minute = minute;
        }
    }

    public enum EventCheckCondition
    {
        EachWeek,
        EachYear,
        AppointTime,
    }
}