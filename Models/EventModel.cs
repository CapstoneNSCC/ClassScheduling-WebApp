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
    public string teacher { get; set; }
    public string Classroom { get; set; }
  }

}
