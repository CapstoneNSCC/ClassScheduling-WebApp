using Microsoft.EntityFrameworkCore;
using ClassScheduling_WebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClassScheduling_WebApp.Models
{
  public class ScheduleManager : DbContext
  {
    // Define DbSet properties for your entities
    public DbSet<EducationalProgram>? EducationalPrograms { get; set; }
    public DbSet<Courses>? Courses { get; set; }

    // ----------------------------------- gets and set
    // Using schedule manager to get the data from the programs table to provide a list object to be used on the index page and elsewhere.
    private DbSet<EducationalProgram>? TblEducationalPrograms { get; set; }
    private DbSet<Courses>? TblCourses { get; set; }

    public List<EducationalProgram> Programs
    {
      get
      {
        if (TblEducationalPrograms != null)
        {
          // Ensure that TblEducationalPrograms is not null before performing LINQ operations
          return TblEducationalPrograms.OrderByDescending(p => p.Name).ToList();
        }
        else
        {
          // Handle the case when TblEducationalPrograms is null
          return new List<EducationalProgram>();
        }
      }
    }

    // Get all courses sorted by name
    public List<Courses> GetAllCourses()
    {
      if (Courses != null)
      {
        // making sure courses isn't null - LINQ operations
        return Courses.OrderBy(c => c.Name).ToList();
      }
      else
      {
        // handle when null
        return new List<Courses>();
      }
    }

    public EducationalProgram? GetProgramByID(int programID)
    {
      // making sure educationalprograms isn't null - before LINQ operations
      if (EducationalPrograms != null)
      {
        // Retrieve the program from the database
        return EducationalPrograms.FirstOrDefault(p => p.Id == programID);
      }
      else
      {
        // handle when null
        return null;
      }
    }
  }
}