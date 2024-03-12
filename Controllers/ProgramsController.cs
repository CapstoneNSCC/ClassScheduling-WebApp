using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Immutable;
using ClassScheduling_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClassScheduling_WebApp.Controllers
{

    public class ProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult AddProgram()
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            // construct course object that will be used to add a new course.
            ProgramModel program = new ProgramModel
            {
                Name = "",
                Year = 1,
            };
            //passing in program model to the view
            return View(program);
        }

        public IActionResult AddSubmit(ProgramModel program)
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            // add the program to the list of programs
            _context.Programs.Add(program);
            //save changes to the database
            _context.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

    }
}