using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Immutable;
using ClassScheduling_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassScheduling_WebApp.Controllers
{
  public class AdminController : Controller
  {
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var programs = _context.Programs
      .Select(p => new ProgramModel
      {
        Id = p.Id,
        Name = p.Name,
        Year = p.Year,
      })
      .OrderByDescending(p => p.Name)
      .ToList();

      var userId = HttpContext.Session.GetInt32("userId");
      var userName = HttpContext.Session.GetString("user");
      ViewBag.currentUserId = userId;
      ViewBag.currentUserName = userName;

      PopulateProgramsDropDownList();
      return View(programs);
    }


    public void PopulateProgramsDropDownList(object selectedProgram = null)
    {
      var programsQuery = from d in _context.Programs
                          orderby d.Name
                          select new
                          {
                            d.Id,
                            ProgramName = d.Name + ": Year " + d.Year
                          };
      ViewBag.IdProgram = new SelectList(programsQuery.AsNoTracking(), "Id", "ProgramName", selectedProgram);
    }

    public IActionResult Logout()
    {
      // clear session data
      HttpContext.Session.Clear();
      return RedirectToAction("Index", "Login");
    }


    public IActionResult AdminFacultyDashboard()
    {
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("index", "Login");
      }

      var userId = HttpContext.Session.GetInt32("userId");
      var userName = HttpContext.Session.GetString("user");
      ViewBag.currentUserId = userId;
      ViewBag.currentUserName = userName;

      PopulateProfessorsDropDownList();
      return View("~/Views/Admin/adminFacultyDashboard.cshtml");
    }

    // reused this method from the Home controller to populate the professors dropdown
    public void PopulateProfessorsDropDownList(object selectedProfessor = null)
    {
      var professorsQuery = from d in _context.Users
                            where d.SetAsAdmin == false
                            orderby d.FirstName
                            select new
                            {
                              d.Id,
                              ProfessorName = d.FirstName + " " + d.LastName
                            };
      ViewBag.IdProfessor = new SelectList(professorsQuery.AsNoTracking(), "Id", "ProfessorName", selectedProfessor);
    }
  }
}
