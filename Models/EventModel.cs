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

    public string title { get; set; }

    public string description { get; set; }

    public string daysOfWeek { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int teacher { get; set; }
    //public virtual UserModel Professor { get; set; }
    public string Classroom { get; set; }

    public int program { get; set; }

    //public virtual ProgramModel Programs { get; set; }
  }

}