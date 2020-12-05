using System;
using System.Collections.Generic;
using System.Text;

namespace internship_3_oop_intro
{
    public class Event
    {
        public Event(string eventName, DateTime startTime, DateTime endTime, int eventType)
        {
            EventName = eventName;
            StartTime = startTime;
            EndTime = endTime;
            EventType = (EventTypeList)eventType;
        }
        public string EventName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public object EventType { get; set; }
        public  enum EventTypeList
        {
            Coffee,
            Lecture,
            Concert,
            StudySession
        }
    }
}
