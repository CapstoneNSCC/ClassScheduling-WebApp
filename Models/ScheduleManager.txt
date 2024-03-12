using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassScheduling_WebApp.Models
{

  public class ScheduleManager : DbContext
  {

    // ----------------------------------- gets and set
    //using schedule manager to get the data from the programs table to provide a list object to be used on the index page and elsewhere.
    private DbSet<EducationalProgram> TblEducationalPrograms { get; set; } = null;

    private DbSet<Courses> TblCourses { get; set; }

    public List<EducationalProgram> Programs
    {
      get
      {
        return TblEducationalPrograms.OrderByDescending(p => p.Name).ToList();
      }
    }

    // get all links sorted by name
    public List<Courses> allCourses
    {
      get
      {
        return TblCourses.OrderBy(c => c.Name).ToList();
      }
    }


    // get a program by its ID
    public EducationalProgram getProgramByID(int programID)
    {
      //using LINQ method to query data and return as list
      //.Single is used to get a single element from the filtered collection. (it assumes there is only 1 in the collection that satisfies the condition if more then on, or none that match it throws an exception. - but i don't need to worry about duplicates in this case as its based off the id which is the primary key)
      return TblEducationalPrograms.Where(p => p.Id == programID).Single();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySql(Connection.CONNECTION_STRING, new MySqlServerVersion(new Version(8, 2, 0)));
    }

  }
}