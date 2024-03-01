using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//added name space to be used with the schedual manager.
namespace ClassScheduling_WebApp.Models
{
    [Table("TblSchedule")]
    public class TblSchedule
    {
        [Key, Column(Order = 0), ForeignKey("Calendar")]
        public int IdCalendar { get; set; }

        [Key, Column(Order = 1), ForeignKey("Course")]
        public int IdCourse { get; set; }

        [Key, Column(Order = 2), ForeignKey("Classroom")]
        public int IdClassroom { get; set; }

        public virtual required TblCalendar Calendar { get; set; }
        public virtual required Courses Course { get; set; }
        public virtual required Classroom Classroom { get; set; }
    }
}

