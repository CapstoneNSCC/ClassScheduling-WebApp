using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ClassScheduling_WebApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Handles GET requests for the schedule index page by fetching all schedules and passing them to the view
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }
            var schedules = await _context.Schedules
                .Include(s => s.Calendar)
                .Include(s => s.Course)
                .Include(s => s.Classroom)
                .ToListAsync();
            return View(schedules);
        }

        // Displays details of a specific schedule identified by a composite key
        public async Task<IActionResult> Details(int idCalendar, int idCourse, int idClassroom)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.IdCalendar == idCalendar && m.IdCourse == idCourse && m.IdClassroom == idClassroom);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // Returns the view for creating a new schedule entry
        public IActionResult Create()
        {
            return View();
        }

        // Processes the creation of a new schedule entry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCalendar,IdCourse,IdClassroom")] ScheduleModel scheduleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(scheduleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(scheduleModel);
        }

        // Returns the view for editing an existing schedule entry
        public async Task<IActionResult> Edit(int idCalendar, int idCourse, int idClassroom)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.IdCalendar == idCalendar && m.IdCourse == idCourse && m.IdClassroom == idClassroom);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // Processes the updates to an existing schedule entry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idCalendar, int idCourse, int idClassroom, [Bind("IdCalendar,IdCourse,IdClassroom")] ScheduleModel scheduleModel)
        {
            if (idCalendar != scheduleModel.IdCalendar || idCourse != scheduleModel.IdCourse || idClassroom != scheduleModel.IdClassroom)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(scheduleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(scheduleModel.IdCalendar, scheduleModel.IdCourse, scheduleModel.IdClassroom))
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
            return View(scheduleModel);
        }

        // Returns the view for deleting a schedule entry
        public async Task<IActionResult> Delete(int idCalendar, int idCourse, int idClassroom)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.IdCalendar == idCalendar && m.IdCourse == idCourse && m.IdClassroom == idClassroom);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // Processes the confirmation of schedule deletion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idCalendar, int idCourse, int idClassroom)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(m => m.IdCalendar == idCalendar && m.IdCourse == idCourse && m.IdClassroom == idClassroom);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Checks if a schedule exists in the database
        private bool ScheduleExists(int idCalendar, int idCourse, int idClassroom)
        {
            return _context.Schedules.Any(e => e.IdCalendar == idCalendar && e.IdCourse == idCourse && e.IdClassroom == idClassroom);
        }

        // The following methods are integrated from the old ProgramModel, adapted to the new model

        // Fetches and displays a list of programs, ordered by name
        public async Task<IActionResult> ListPrograms()
        {
            var programs = await _context.Programs.OrderByDescending(p => p.Name).ToListAsync();
            return View("ListPrograms", programs); // Assumes a corresponding view exists
        }

        // Fetches and displays a list of all courses, ordered by name
        public async Task<IActionResult> ListAllCourses()
        {
            var courses = await _context.Courses.OrderBy(c => c.Name).ToListAsync();
            return View("ListAllCourses", courses); // Assumes a corresponding view exists
        }

        // Fetches and displays details of a specific program by ID
        public async Task<IActionResult> GetProgramById(int programId)
        {
            var program = await _context.Programs.FirstOrDefaultAsync(p => p.Id == programId);
            if (program == null)
            {
                return NotFound();
            }
            return View("GetProgramById", program); // Assumes a corresponding view exists
        }
    }
}
