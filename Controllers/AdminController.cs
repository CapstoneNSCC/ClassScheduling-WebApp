using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Data;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

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
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            var schedules = _context.Schedules.ToList(); 
            return View(schedules);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
