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

    [HttpPost]
    public async Task<IActionResult> GetSchedule()
    {
        if (HttpContext.Session.GetString("auth") != "true")
        {
            return RedirectToAction("Index", "Login");
        }

        try
        {
            _context.TblEvents.RemoveRange(_context.TblEvents);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao excluir registros: {ex.Message}");
        }

        var calendars = _context.Calendars.ToList();

        /*
          The below code is this script

            select * from TblCourse tcourse
              inner join TblTechClass tclass on tcourse.Id = tclass.IdCourse
              inner join TblTechRoom troom on tclass.IdTechnology = troom.IdTechnology 
              inner join TblClassroom tclassroom on tclassroom.Id = troom.IdClassroom;
        */
        var query = _context.Courses
        .Join(_context.TechClasses, 
              course => course.Id, 
              techClass => techClass.IdCourse,
              (course, techClass) => new { Course = course, TechClass = techClass })
        .Join(_context.TechRooms,
              joined => joined.TechClass.IdTechnology,
              techRoom => techRoom.IdTechnology,
              (joined, techRoom) => new { Joined = joined, TechRoom = techRoom })
        .Join(_context.Classrooms,
              joined => joined.TechRoom.IdClassroom,
              classroom => classroom.Id,
              (joined, classroom) => new { Joined = joined, Classroom = classroom })
        .Select(result => new 
        {
            Course = result.Joined.Joined.Course,
            Classroom = result.Classroom
        })
        .ToList();

        foreach (var eventData in query)
        {
          foreach (var calendar in calendars)
          {
            DateTime startDateTime = DateTime.Today.Add(calendar.StartTime);
            DateTime endDateTime = startDateTime.AddHours(2); 

            var eventModel = new EventModel
            {
                courseCode = eventData.Course.Code,
                courseName = eventData.Course.Name,
                daysOfWeek = GetDayOfWeekValue(calendar.DayWeek),
                startTime = startDateTime,
                endTime = endDateTime,
                professor = eventData.Course.IdProfessor,
                classroom = eventData.Classroom.RoomNumber.ToString(),
                program = eventData.Course.IdProgram
            };
            
            _context.TblEvents.Add(eventModel);
          }
        }

        await _context.SaveChangesAsync();

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

    private string GetDayOfWeekValue(string dayOfWeekName)
    {
      switch (dayOfWeekName.ToLower())
      {
        case "monday":
            return "1";
        case "tuesday":
            return "2";
        case "wednesday":
            return "3";
        case "thursday":
            return "4";
        case "friday":
            return "5";
        default:
            throw new ArgumentException("Invalid day of week name.");
      }
    }
  }
}