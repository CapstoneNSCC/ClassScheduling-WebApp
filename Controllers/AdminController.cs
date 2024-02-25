using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Immutable;

namespace ClassScheduling_WebApp.Controllers
{

  public class AdminController : Controller
  {
    private ScheduleManager scheduleManager;

    public AdminController(ScheduleManager myManager)
    {
      scheduleManager = myManager;
    }

    public IActionResult Index()
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      return View(scheduleManager);
    }

    public IActionResult Logout()
    {
      // Clear session data
      HttpContext.Session.Clear();
      return RedirectToAction("Index", "Login");
    }


    [Route("/Admin/AddProgram")]
    public IActionResult AddProgram()
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // construct course object that will be used to add a new course.
      EducationalProgram program = new EducationalProgram
      {
        Name = "",
      };
      return View(scheduleManager);
    }

    [Route("/Admin/AddCourse/{programID:int}")]
    public IActionResult AddCourse(int programID)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // construct course object that will be used to add a new course.
      Courses course = new Courses
      {
        Code = "",
        Name = "",
        Professor = new User(),
        EducationalProgram = scheduleManager.getProgramByID(programID)
      };

      //storing SelectList object in ViewBag
      ViewBag.EducationalProgram = scheduleManager.getProgramByID(programID);
      course.IdEducationalProgram = programID;
      // pass it into the view for populating
      return View(scheduleManager);
    }
  }
}