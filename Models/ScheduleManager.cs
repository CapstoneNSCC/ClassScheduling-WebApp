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
    //using schedual manager to get the data from the programs table to provide a list object to be used on the index page and elseware.
    private DbSet<EducationalProgram> TblEducationalPrograms { get; set; } = null;

    public List<EducationalProgram> Programs
    {
      get
      {
        return TblEducationalPrograms.ToList();
      }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseMySql(Connection.CONNECTION_STRING, new MySqlServerVersion(new Version(8, 2, 0)));
    }

  }
}