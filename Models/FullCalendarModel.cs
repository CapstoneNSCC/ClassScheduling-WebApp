using Microsoft.EntityFrameworkCore;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassScheduling_WebApp.Models
{
public class FullCalendarModel
{
    public string courseCode { get; set; }

    public string courseName { get; set; }

    public string daysOfWeek { get; set; }
    public string[] daysOfWeekArray { get; set; }

    public DateTime startTime { get; set; }
    public DateTime endTime { get; set; }
    public string startTimeStr { get; set; }
    public string endTimeStr { get; set; }
    public string professor { get; set; }
    public int professorId { get; set; }
    public string classroom { get; set; }

    public string program { get; set; }
    public int programId { get; set; }

    //public virtual UserModel Professor { get; set; }
    //public virtual ProgramModel Programs { get; set; }
  }

}