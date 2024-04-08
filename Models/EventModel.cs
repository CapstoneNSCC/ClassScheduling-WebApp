using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{
  [Table("TblEvents")]
  public class EventModel
  {
    public int Id { get; set; }

    public string courseCode { get; set; }

    public string courseName { get; set; }

    public string daysOfWeek { get; set; }

    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public int professor { get; set; }
    public string classroom { get; set; }

    public int program { get; set; }

    //public virtual UserModel Professor { get; set; }
    //public virtual ProgramModel Programs { get; set; }
  }

}