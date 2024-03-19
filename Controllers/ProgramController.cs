using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace ClassScheduling_WebApp.Controllers
{
    public class ProgramController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        // Fetches and displays details of a specific program by ID
        public async Task<IActionResult> GetProgramById(int programId)
        {
            var program = await _context.Programs
                .Include(p => p.Courses) // Including related Courses to the Program
                .FirstOrDefaultAsync(p => p.Id == programId);
            
            if (program == null)
            {
                return NotFound();
            }

            return View("GetProgramById", program); // Ensure you have a View for this
        }

        // Lists all courses for a specific program
        public async Task<IActionResult> ListAllCoursesForProgram(int programId)
        {
            var programWithCourses = await _context.Programs
                .Where(p => p.Id == programId)
                .Include(p => p.Courses)
                .FirstOrDefaultAsync();
            
            if (programWithCourses == null)
            {
                return NotFound();
            }

            // Assuming you have a view that expects a ProgramModel and displays its courses
            return View("ListAllCoursesForProgram", programWithCourses); 
        }

        // Method to check if a ProgramModel exists, using its ID
        private bool ProgramModelExists(int id)
        {
            return _context.Programs.Any(e => e.Id == id);
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

        [Route("/Programs/Update/{programID:int}")]
        public IActionResult Update(int programID)
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            // find the program by the programID
            ProgramModel program = _context.Programs.Find(programID);
            //passing in program model to the view
            return View("EditProgram", program);
        }

        public IActionResult UpdateSubmit(ProgramModel program)
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            // update the program in the list of programs
            _context.Programs.Update(program);
            //save changes to the database
            _context.SaveChanges();
            return RedirectToAction("Index", "Admin");
        }

        [Route("/Programs/Delete/{programID:int}")]
        public IActionResult Delete(int programID)
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            // find the program by the programID
            ProgramModel program = _context.Programs.Find(programID);

            return View("DeleteProgram", program);
        }

        public IActionResult DeleteSubmit(ProgramModel program)
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }
            var existingProgram = _context.Programs.Find(program.Id);
            if (existingProgram == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            // remove the program from the list of programs
            _context.Programs.Remove(existingProgram);
            //save changes to the database
            _context.SaveChanges();
            return RedirectToAction("Index", "Admin");

        }
    }
}
