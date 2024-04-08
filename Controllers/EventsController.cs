using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ClassScheduling_WebApp.Controllers
{

  public class EventsController : Controller
  {
    private readonly ApplicationDbContext _context;

    public EventsController(ApplicationDbContext context)
    {
      _context = context;
    }

    //GET: api/Events
    [HttpGet]
    [Route("/api/Events")]
    public IActionResult GetEvents()
    {
      var events = _context.TblEvents.Select(e => new
      {
        courseCode = e.courseCode,
        courseName = e.courseName,
        daysOfWeek = new[] { e.daysOfWeek.ToString() },
        startTime = e.startTime.ToString("HH:mm:ss"),
        endTime = e.endTime.ToString("HH:mm:ss"),
        professor = (from user in _context.Users
                     where user.Id == e.professor
                     select user.FirstName + " " + user.LastName)
                    .ToList(),
        classroom = e.classroom,
        program = (
            from program in _context.Programs
            where program.Id == e.program
            select program.Name + ": Year " + program.Year
        )
        .ToList()
      })
      .ToList();

      return Json(events);
    }

    //GET: api/Events/teacher/{id}
    [HttpGet]
    [Route("/api/Events/teacher/{id}")]
    public IActionResult GetEventsByTeacher(int id)
    {
      var events = _context.TblEvents
          .Where(e => e.professor == id)
          .Select(e => new
          {
            courseCode = e.courseCode,
            courseName = e.courseName,
            daysOfWeek = new[] { e.daysOfWeek.ToString() },
            startTime = e.startTime.ToString("HH:mm:ss"),
            endTime = e.endTime.ToString("HH:mm:ss"),
            professor = (from user in _context.Users
                         where user.Id == e.professor
                         select user.FirstName + " " + user.LastName)
                    .ToList(),
            classroom = e.classroom,
            program = (
            from program in _context.Programs
            where program.Id == e.program
            select program.Name + ": Year " + program.Year
        )
        .ToList()
          })
          .ToList();

      return Json(events);
    }

    //GET: api/Events/program/{id}
    [HttpGet]
    [Route("/api/Events/program/{id}")]
    public IActionResult GetEventsByProgram(int id)
    {
      var events = _context.TblEvents
          .Where(e => e.program == id)
          .Select(e => new
          {
            courseCode = e.courseCode,
            courseName = e.courseName,
            daysOfWeek = new[] { e.daysOfWeek.ToString() },
            startTime = e.startTime.ToString("HH:mm:ss"),
            endTime = e.endTime.ToString("HH:mm:ss"),
            professor = (from user in _context.Users
                         where user.Id == e.professor
                         select user.FirstName + " " + user.LastName)
                    .ToList(),
            classroom = e.classroom,
            program = (
            from program in _context.Programs
            where program.Id == e.program
            select program.Name + ": Year " + program.Year
        )
        .ToList()
          })
          .ToList();

      return Json(events);
    }
  }
}