using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{
    [Table("TblCalendar")]
    public class CalendarModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Day of the week is required.")]
        public string DayWeek { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }
    }

}