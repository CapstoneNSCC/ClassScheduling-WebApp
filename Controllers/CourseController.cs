using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering; // Required for SelectList
using Microsoft.AspNetCore.Http; // Required for Session

namespace ClassScheduling_WebApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Displays a list of all courses
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            var courses = await _context.Courses.Include(c => c.Professor).Include(c => c.TechClasses).ToListAsync();
            return View(courses);
        }

        // Displays details for a specific course
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Professor)
                .Include(c => c.TechClasses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // // Shows the form to add a new course
        // public IActionResult Add()
        // {
        //     if (HttpContext.Session.GetString("auth") != "true")
        //     {
        //         return RedirectToAction("Index", "Login");
        //     }

        //     PopulateProfessorsDropDownList();
        //     PopulateProgramsDropDownList();
        //     return View(new CourseModel()); // Passes an empty instance to the View
        // }

        
        // Shows the form to add a new course for a specific program
        public IActionResult AddCourse(int? programId)
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            PopulateProfessorsDropDownList();
            PopulateProgramsDropDownList(programId); // Pass the programId to pre-select the program in the dropdown
            
            var courseModel = new CourseModel();
            if (programId.HasValue)
            {
                courseModel.IdProgram = programId.Value; // Pre-select the program if an ID was provided
            }
            
            return View("~/Views/Course/AddCourse.cshtml", courseModel); // Use the same Add view, but pass in the pre-selected program
        }


        // POST: Adds a new course to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add([Bind("Id,Code,Name,Hours,IdProfessor,IdProgram")] CourseModel course)
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateProfessorsDropDownList(course.IdProfessor);
            PopulateProgramsDropDownList(course.IdProgram);
            return View(course);
        }

        // Shows the form to edit an existing course
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            PopulateProfessorsDropDownList(course.IdProfessor);
            PopulateProgramsDropDownList(course.IdProgram);
            return View(course);
        }

        // POST: Updates an existing course in the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Hours,IdProfessor,IdProgram")] CourseModel course)
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateProfessorsDropDownList(course.IdProfessor);
            PopulateProgramsDropDownList(course.IdProgram);
            return View(course);
        }

        // Shows a confirmation page for course deletion
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Deletes a course from the database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            var course = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        private void PopulateProfessorsDropDownList(object selectedProfessor = null)
        {
            var professorsQuery = from d in _context.Users
                                where d.SetAsAdmin == false
                                orderby d.FirstName
                                select new
                                {
                                    d.Id,
                                    ProfessorName = d.FirstName + " " + d.LastName
                                };
            ViewBag.IdProfessor = new SelectList(professorsQuery.AsNoTracking(), "Id", "ProfessorName", selectedProfessor);
        }

        private void PopulateProgramsDropDownList(object selectedProgram = null)
        {
            var programsQuery = from p in _context.Programs
                                orderby p.Name
                                select p;
            ViewBag.IdProgram = new SelectList(programsQuery.AsNoTracking(), "Id", "Name", selectedProgram);
        }
    }
}
