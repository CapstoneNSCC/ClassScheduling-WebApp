using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;

namespace ClassScheduling_WebApp.Controllers
{

  public class PublicController : Controller
  {
    private ScheduleManager scheduleManager;

    public PublicController(ScheduleManager myManager)
    {
      scheduleManager = myManager;
    }

    public IActionResult Index()
    {
      return View(scheduleManager);
    }

  }
}