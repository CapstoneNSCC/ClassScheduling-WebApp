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

            var classrooms = _context.Classrooms
            .Select(c => new ClassroomModel
            {
                Id = c.Id,
                RoomNumber = c.RoomNumber,
                BuildingAcronym = c.BuildingAcronym,
                // Include other properties as needed
            })
            .OrderByDescending(c => c.RoomNumber)
            .ToList();



            var viewModel = new IndexViewModel
            {
                Programs = programs,
                Technologies = technologies,
                Users = users,
                Classrooms = classrooms,
            };

            return View(viewModel);
        }

        public IActionResult Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }


        //[Route("/Admin/AddCourse/{programID:int}")]
        // public IActionResult AddCourse(int programID)
        // {
        //     // if auth is not  = true, it re-directs to the login screen.
        //     if (HttpContext.Session.GetString("auth") != "true")
        //     {
        //         return RedirectToAction("Index", "Login");
        //     }

        //     // construct course object that will be used to add a new course.
        //     Courses course = new Courses
        //     {
        //         Code = "",
        //         Name = "",
        //         Professor = new User(),
        //         EducationalProgram = scheduleManager.getProgramByID(programID)
        //     };

        //     //storing SelectList object in ViewBag
        //     ViewBag.EducationalProgram = scheduleManager.getProgramByID(programID);
        //     course.IdEducationalProgram = programID;
        //     // pass it into the view for populating
        //     return View(scheduleManager);
        // }
    }
}
