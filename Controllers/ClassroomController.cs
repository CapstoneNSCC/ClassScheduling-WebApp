using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Immutable;
using ClassScheduling_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace ClassScheduling_WebApp.Controllers
{
  public class ClassroomController : Controller
  {
    private readonly ApplicationDbContext _context;

    public ClassroomController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var classrooms = _context.Classrooms
      .Select(c => new ClassroomModel
      {
        Id = c.Id,
        RoomNumber = c.RoomNumber,
        BuildingAcronym = c.BuildingAcronym
      })
      .OrderByDescending(c => c.RoomNumber)
      .ToList();

      var viewModel = new IndexViewModel
      {
        Classrooms = classrooms
      };

      return View(viewModel);
    }
    public IActionResult AddRoom()
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      ViewBag.Technologies = _context.Technologies.ToList();


      // construct course object that will be used to add a new course.
      ClassroomModel classroom = new ClassroomModel
      {
        RoomNumber = 0,
        BuildingAcronym = "",
      };

      //passing in technology model to the view
      return View(classroom);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubmit([Bind("Id, RoomNumber, BuildingAcronym, SelectedTechnologyIds")] ClassroomModel classroom, List<int> SelectedTechnologyIds)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      if (ModelState.IsValid){
        _context.Add(classroom);
        await _context.SaveChangesAsync();

        foreach (var techId in SelectedTechnologyIds)
        {
          var techRoom = new TechRoomModel { IdClassroom = classroom.Id, IdTechnology = techId };
          _context.TechRooms.Add(techRoom);
        }

        await _context.SaveChangesAsync();

        var classrooms = _context.Classrooms
        .Select(c => new ClassroomModel
        {
          Id = c.Id,
          RoomNumber = c.RoomNumber,
          BuildingAcronym = c.BuildingAcronym
        })
        .OrderByDescending(c => c.RoomNumber)
        .ToList();

        var viewModel = new IndexViewModel
        {
          Classrooms = classrooms
        };

        return View("Index", viewModel);
      }

      ViewBag.Technologies = _context.Technologies.ToList();

      return View("~/Views/Classroom", classroom);
    }


    [Route("/Classroom/Update/{ClassroomID:int}")]
    public IActionResult Update(int ClassroomID)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // find the technology by the technologyID
      ClassroomModel classroom = _context.Classrooms.Find(ClassroomID);

      ViewBag.Technologies = _context.Technologies.ToList();

      classroom.SelectedTechnologyIds = _context.TechRooms
          .Where(tr => tr.IdClassroom == ClassroomID)
          .Select(tr => tr.IdTechnology)
          .ToList();

      //passing in technology model to the view
      return View("EditRoom", classroom);
    }


    public async Task<IActionResult> UpdateSubmit(int id, [Bind("Id, RoomNumber, BuildingAcronym, SelectedTechnologyIds")] ClassroomModel classroom, List<int> SelectedTechnologyIds)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      if (id != classroom.Id)
      {
        return NotFound();
      }

      if (ModelState.IsValid){
        try
        {
          _context.Update(classroom);

          var existingTechRooms = _context.TechRooms.Where(tr => tr.IdClassroom == classroom.Id);
          _context.TechRooms.RemoveRange(existingTechRooms);

          foreach (var techId in SelectedTechnologyIds)
          {
            var techRoom = new TechRoomModel { IdClassroom = classroom.Id, IdTechnology = techId };
            _context.TechRooms.Add(techRoom);
          }

          await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
          if (!(_context.Classrooms.Any(e => e.Id == classroom.Id)))
          {
            return NotFound();
          }
          else
          {
            throw;
          }
        }

        return RedirectToAction("Index", classroom);
      }

      ViewBag.Technologies = _context.Technologies.ToList(); // Add this line again for model validation fail scenario
      return View(classroom);
    }

    [Route("/Classroom/Delete/{ClassroomID:int}")]
    public IActionResult Delete(int ClassroomID)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // find the technology by the technologyID
      ClassroomModel classroom = _context.Classrooms.Find(ClassroomID);

      return View("DeleteRoom", classroom);

    }


     // POST: Deletes a course from the database
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSubmit(int id)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var techRooms = _context.TechRooms.Where(tr => tr.IdClassroom == id);
      _context.TechRooms.RemoveRange(techRooms);

      var classroom = await _context.Classrooms.FindAsync(id);
      _context.Classrooms.Remove(classroom);

      await _context.SaveChangesAsync();

      return RedirectToAction("Index", classroom);
    }
  }
}