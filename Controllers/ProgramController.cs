using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClassScheduling_WebApp.Controllers
{
    public class ProgramController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProgramController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Existing CRUD methods here...

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
    }
}
