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
            return View(programs);
        }

        public IActionResult Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }


        // [Route("/Admin/AddProgram")]
        // public IActionResult AddProgram()
        // {
        //     // if auth is not  = true, it re-directs to the login screen.
        //     if (HttpContext.Session.GetString("auth") != "true")
        //     {
        //         return RedirectToAction("Index", "Login");
        //     }

        //     // construct course object that will be used to add a new course.
        //     ProgramModel program = new ProgramModel
        //     {
        //         Name = "",
        //         Year = 1,
        //     };
        //     //passing in program model to the view
        //     return View(program);
        // }

        // public IActionResult AddSubmit(ProgramModel program)
        // {
        //     // if auth is not  = true, it re-directs to the login screen.
        //     if (HttpContext.Session.GetString("auth") != "true")
        //     {
        //         return RedirectToAction("Index", "Login");
        //     }

        //     // add the program to the list of programs
        //     //ProgramModel.Add(program);
        //     //save changes to the database
        //     //scheduleManager.SaveChanges();
        //     return RedirectToAction("Index", "Admin");
        // }

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
