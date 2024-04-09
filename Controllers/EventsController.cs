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
    
    private static List<FullCalendarModel> _eventList = new List<FullCalendarModel> {};

    public EventsController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
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
          Console.WriteLine($"Error to exclude the data: {ex.Message}");
      }
      
      var eventOptions = GetSchedule();

      var viewModel = new IndexViewModel
      {
        EventOptions = eventOptions
      };
      
      return View(viewModel);
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
    [Route("api/[controller]/chooseEventOption")]
    public async Task<IActionResult> chooseEventOption([FromBody]List<FullCalendarModel> fullCalendarEvents)
    {
      if (HttpContext.Session.GetString("admin") != "true")
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

      foreach (var eventData in fullCalendarEvents){
         var eventModel = new EventModel
                {
                    courseCode = eventData.courseCode,
                    courseName = eventData.courseName,
                    daysOfWeek = eventData.daysOfWeek,
                    startTime = eventData.startTime,
                    endTime = eventData.endTime,
                    professor = eventData.professorId,
                    classroom = eventData.classroom,
                    program = eventData.programId,
                };
    
                _context.TblEvents.Add(eventModel);
      }
      
      await _context.SaveChangesAsync();

      return RedirectToAction("Index", "Admin");
    }

    [HttpPost]
    public Dictionary<string, List<FullCalendarModel>> GetSchedule()
    {
        var eventOptions = new Dictionary<string, List<FullCalendarModel>>
        {
            { "Option1", new List<FullCalendarModel>() },
            { "Option2", new List<FullCalendarModel>() },
            { "Option3", new List<FullCalendarModel>() }
        };
        
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

        for (int i = 0; i < 3; i++)
        {
          _eventList.Clear();
          
          var calendars = _context.Calendars.ToList().OrderBy(x => Random.Shared.Next()).ToList();

          foreach (var eventData in query)
          {
            foreach (var calendar in calendars)
            {
              DateTime startDateTime = DateTime.Today.Add(calendar.StartTime);
              DateTime endDateTime = startDateTime.AddHours(2); 

              bool professorAvailable = IsProfessorAvailable(eventData.Course.IdProfessor, GetDayOfWeekValue(calendar.DayWeek), startDateTime, endDateTime);

              bool classroomAvailable = IsClassroomAvailable(eventData.Classroom.Id.ToString(), GetDayOfWeekValue(calendar.DayWeek), startDateTime, endDateTime, eventData.Course.IdProgram);

              bool withinTimeLimit = IsWithinTimeLimit(eventData.Course.Code, eventData.Course.IdProgram, GetDayOfWeekValue(calendar.DayWeek), eventData.Course.Hours);
              
              if (professorAvailable && classroomAvailable && withinTimeLimit)
              {
                UserModel professor = _context.Users.Find(eventData.Course.IdProfessor);

                ProgramModel program = _context.Programs.Find(eventData.Course.IdProgram);

                var eventModel = new FullCalendarModel
                {
                    courseCode = eventData.Course.Code,
                    courseName = eventData.Course.Name,
                    daysOfWeekArray = new[] { GetDayOfWeekValue(calendar.DayWeek) },
                    startTimeStr = startDateTime.ToString("HH:mm:ss"),
                    endTimeStr = endDateTime.ToString("HH:mm:ss"),
                    daysOfWeek = GetDayOfWeekValue(calendar.DayWeek),
                    startTime = startDateTime,
                    endTime = endDateTime,
                    professor = professor.FirstName,
                    professorId = eventData.Course.IdProfessor,
                    classroom = eventData.Classroom.RoomNumber.ToString(),
                    programId = eventData.Course.IdProgram,
                    program = program.Name,
                };
                
                _eventList.Add(eventModel);
              }
            }
          }

          eventOptions[$"Option{i + 1}"].AddRange(_eventList);
        }

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

        return eventOptions;
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

    private bool IsProfessorAvailable(int professorId, string daysOfWeek, DateTime startTime, DateTime endTime)
    {
        return !_eventList.Any(e =>
            e.professorId == professorId &&
            e.daysOfWeek == daysOfWeek &&
            e.startTime <= startTime && e.endTime >= endTime);
    }

    private bool IsClassroomAvailable(string classroom, string daysOfWeek, DateTime startTime, DateTime endTime, int programId)
    {
        return !_eventList.Any(e =>
            e.classroom == classroom &&
            e.daysOfWeek == daysOfWeek &&
            e.startTime <= startTime && e.endTime >= endTime &&
            e.programId == programId);
    }

    private bool IsWithinTimeLimit(string courseCode, int programId, string daysOfWeek, int hours)
    {
        if (_eventList.Any(e => e.courseCode == courseCode && e.programId == programId && e.daysOfWeek == daysOfWeek))
        {
            return false; 
        }

        int eventosComMesmoHorario = _eventList.Count(e => e.courseCode == courseCode);

        if (hours == 60 && eventosComMesmoHorario < 2)
        {
            return true; 
        }
        else if (hours == 90 && eventosComMesmoHorario < 3)
        {
            return true; 
        }
        else
        {
            return false;
        }
    }

  }

}