using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Immutable;
using ClassScheduling_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

            var programs = _context.Programs
            .Select(p => new ProgramModel
            {
                Id = p.Id,
                Name = p.Name,
                Year = p.Year,
                // Include other properties as needed
            })
            .OrderByDescending(p => p.Name)
            .ToList();

            var technologies = _context.Technologies
            .Select(t => new TechnologyModel
            {
                Id = t.Id,
                Description = t.Description,
                // Include other properties as needed
            })
            .OrderByDescending(t => t.Description)
            .ToList();

            var users = _context.Users
            .Select(u => new UserModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                SetAsAdmin = u.SetAsAdmin,
                UserName = u.UserName,
                Password = u.Password,
                Salt = u.Salt,
                // Include other properties as needed
            })
            .OrderByDescending(u => u.UserName)
            .ToList();



            var viewModel = new IndexViewModel
            {
                Programs = programs,
                Technologies = technologies,
                Users = users
            };

            return View(viewModel);
        }

        public IActionResult Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
