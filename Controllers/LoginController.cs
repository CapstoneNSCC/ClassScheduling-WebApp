using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using Microsoft.AspNetCore.Http;
using ClassScheduling_WebApp.Data; 

namespace ClassScheduling_WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context; 

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Submit(string myUsername, string myPassword)
        {
            WebLogin webLogin = new WebLogin(_context, HttpContext)
            {
                //update properties
                Username = myUsername,
                Password = myPassword
            };

            if (webLogin.Unlock())
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["feedback"] = "Incorrect Username and/or Password. Please try again...";
                return View("Index");
            }
        }
    }
}
