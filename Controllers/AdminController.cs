using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Generic;

namespace ClassScheduling_WebApp.Controllers
{
  public class AdminController : Controller
  {
    private readonly ScheduleManager scheduleManager;

    public AdminController(ScheduleManager myManager)
    {
      scheduleManager = myManager;
    }

    public IActionResult Index()
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      return View(scheduleManager);
    }

    public IActionResult Logout()
    {
      HttpContext.Session.Clear();
      return RedirectToAction("Index", "Login");
    }

    [Route("/Admin/AddProgram")]
    public IActionResult AddProgram()
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      EducationalProgram program = new EducationalProgram
      {
        Name = "",
        Year = 1,
      };

      return View(program);
    }

    [HttpPost]
    public IActionResult AddSubmit(EducationalProgram program)
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      scheduleManager.Add(program);
      scheduleManager.SaveChanges();
      return RedirectToAction("Index", "Admin");
    }

    [Route("/Admin/AddCourse/{programID:int}")]
    public IActionResult AddCourse(int programID)
    {
      var program = scheduleManager.GetProgramByID(programID);
      var courses = scheduleManager.GetAllCourses();

      return View(Tuple.Create(program, courses));
    }

    [HttpPost]
    public IActionResult AddCourseSubmit(Courses course)
    {
      if (ModelState.IsValid)
      {
        if (scheduleManager != null && scheduleManager.Courses != null)
        {
          scheduleManager.Courses.Add(course);
          scheduleManager.SaveChanges();
          return RedirectToAction("Index", "Admin");
        }
        else
        {
          return RedirectToAction("Error", "Home");
        }
      }
      else
      {
        return View("AddCourse", course);
      }
    }
  }
}