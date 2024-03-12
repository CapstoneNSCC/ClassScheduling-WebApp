using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models{

    [Table("TblSchedule")]
    public class ScheduleModel
    {
        [ForeignKey("Calendar")]
        public int IdCalendar { get; set; }

        [ForeignKey("Course")]
        public int IdCourse { get; set; }

        [ForeignKey("Classroom")]
        public int IdClassroom { get; set; }

        // Navigation properties
        public virtual CalendarModel Calendar { get; set; }
        public virtual CourseModel Course { get; set; }
        public virtual ClassroomModel Classroom { get; set; }
    }

}