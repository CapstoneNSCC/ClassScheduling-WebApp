using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; // Needed for List<T>
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

    // Shows the form to add a new course
    public IActionResult AddCourse(int programId)
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      PopulateProfessorsDropDownList();
      PopulateProgramsDropDownList(programId);
      ViewBag.Technologies = _context.Technologies.ToList();

      var course90Hours = getCourse90Hours(programId);

      var courseModel = new CourseModel();

      if (course90Hours != null)
      {
          courseModel = new CourseModel
          {
              IdProgram = programId,
              Hours = 60
          };

          ViewBag.Block90Hours = true;
          
          ViewBag.Message = "There is already a course with a workload of 90 hours. The default workload has been set to 60 hours.";
      }else{
        courseModel = new CourseModel
        {
          IdProgram = programId // Pre-select the program
        };
      }
      
      return View("~/Views/Course/AddCourse.cshtml", courseModel);
    }

    // POST: Adds a new course to the database
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubmit([Bind("Id,Code,Name,Hours,IdProfessor,IdProgram, SelectedTechnologyIds")] CourseModel course, List<int> SelectedTechnologyIds)
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // if (ModelState.IsValid)
      ModelState.Remove("Professor");
      ModelState.Remove("Programs");

      if (!TryValidateModel(course))
      {
        _context.Add(course);
        await _context.SaveChangesAsync();

        foreach (var techId in SelectedTechnologyIds)
        {
          var techClass = new TechClassModel { IdCourse = course.Id, IdTechnology = techId };
          _context.TechClasses.Add(techClass);
        }
        await _context.SaveChangesAsync();

        ProgramModel program = _context.Programs
          .Include(p => p.Courses)
          .FirstOrDefault(p => p.Id == course.IdProgram);
        return View("~/Views/Programs/SingleProgram.cshtml", program);
      }

      PopulateProfessorsDropDownList(course.IdProfessor);
      PopulateProgramsDropDownList(course.IdProgram);
      ViewBag.Technologies = _context.Technologies.ToList();

      return View("~/Views/Course/AddCourse.cshtml", course);
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

      var course90Hours = getCourse90Hours(course.IdProgram);
     
      if (course90Hours != null && course90Hours.Id != course.Id)
      {
          ViewBag.Block90Hours = true;
          
          ViewBag.Message = "There is already a course with a workload of 90 hours. The default workload has been set to 60 hours.";
      }

      PopulateProfessorsDropDownList(course.IdProfessor);
      PopulateProgramsDropDownList(course.IdProgram);
      ViewBag.Technologies = _context.Technologies.ToList();
      ViewBag.Technologies = _context.Technologies.ToList();

      course.SelectedTechnologyIds = _context.TechClasses
          .Where(tc => tc.IdCourse == id)
          .Select(tc => tc.IdTechnology)
          .ToList();

      return View("~/Views/Course/EditCourse.cshtml", course);
    }

    // POST: Updates an existing course in the database
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSubmit(int id, [Bind("Id,Code,Name,Hours,IdProfessor,IdProgram, SelectedTechnologyIds")] CourseModel course, List<int> SelectedTechnologyIds)
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      if (id != course.Id)
      {
        return NotFound();
      }

      ModelState.Remove("Professor");
      ModelState.Remove("Programs");

      if (!TryValidateModel(course))
      {
        try
        {
          _context.Update(course);

          var existingTechClasses = _context.TechClasses.Where(tc => tc.IdCourse == course.Id);
          _context.TechClasses.RemoveRange(existingTechClasses);

          foreach (var techId in SelectedTechnologyIds)
          {
            var techClass = new TechClassModel { IdCourse = course.Id, IdTechnology = techId };
            _context.TechClasses.Add(techClass);
          }

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
        ProgramModel program = _context.Programs
          .Include(p => p.Courses)
          .FirstOrDefault(p => p.Id == course.IdProgram);

        return View("~/Views/Programs/SingleProgram.cshtml", program);
      }

      PopulateProfessorsDropDownList(course.IdProfessor);
      PopulateProgramsDropDownList(course.IdProgram);
      ViewBag.Technologies = _context.Technologies.ToList();
      return View(course);
    }

    // shows confirmation page for delete course
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

      return View("~/Views/Course/DeleteCourse.cshtml", course);
    }

    // POST: Deletes a course from the database
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSubmit(int id)
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var techClasses = _context.TechClasses.Where(tc => tc.IdCourse == id);
      _context.TechClasses.RemoveRange(techClasses);

      var course = await _context.Courses.FindAsync(id);
      _context.Courses.Remove(course);
      await _context.SaveChangesAsync();
      ProgramModel program = _context.Programs
          .Include(p => p.Courses)
          .FirstOrDefault(p => p.Id == course.IdProgram);
      return View("~/Views/Programs/SingleProgram.cshtml", program);
    }

    private bool CourseExists(int id)
    {
      return _context.Courses.Any(e => e.Id == id);
    }

    public void PopulateProfessorsDropDownList(object selectedProfessor = null)
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
      int? programId = selectedProgram as int?;
      var programsQuery = from p in _context.Programs
                          where p.Id == programId || programId == null // Allows for all programs if no ID is specified
                          select new
                          {
                              Id = p.Id,
                              Name = $"{p.Name}, year {p.Year}"
                          };
      ViewBag.IdProgram = new SelectList(programsQuery.AsNoTracking(), "Id", "Name", selectedProgram);
    }

    private CourseModel getCourse90Hours(int IdProgram)
    {
      return _context.Courses.Where(c => c.Hours == 90)
                                                  .Join(
                                                      _context.Programs,
                                                      c => c.IdProgram,
                                                      p => p.Id,
                                                      (c, p) => new { Course = c, Program = p }
                                                  )
                                                  .Where(cp => cp.Program.Id == IdProgram)
                                                  .Select(cp => cp.Course)
                                                  .FirstOrDefault();
    }
    
  }
}