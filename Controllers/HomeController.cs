using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClassScheduling_WebApp.Controllers
{
  public class HomeController : Controller
  {
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult HomeIndex()
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("HomeIndex", "Login");
      }

      var userId = HttpContext.Session.GetInt32("userId");
      var userName = HttpContext.Session.GetString("user");
      ViewBag.currentUserId = userId;
      ViewBag.currentUserName = userName;

      // Reused the PopulateProfessorsDropDownList method from the course controller
      PopulateProfessorsDropDownList();
      return View("~/Views/Home/Index.cshtml");
    }

    // reused this method from the course controller to populate the professors dropdown
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