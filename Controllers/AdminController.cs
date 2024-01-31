using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;

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
  }
}