using Microsoft.EntityFrameworkCore;

using System;

namespace ClassScheduling_WebApp.Models
{
    public class EventModel
    {
        public int Id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public int daysOfWeek { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string teacher { get; set; }
        public string Classroom { get; set; }
    }

}
