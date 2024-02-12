using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TblCalendar")]
public class TblCalendar
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Day of the week is required.")]
    public required string DayWeek { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public TimeSpan StartTime { get; set; }
}
