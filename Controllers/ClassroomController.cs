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


    public IActionResult UpdateSubmit(ClassroomModel classroom)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // update the program in the list of programs
      _context.Classrooms.Update(classroom);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("Index", classroom);
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

    public IActionResult DeleteSubmit(ClassroomModel classroom)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var existingRoom = _context.Classrooms.Find(classroom.Id);
      if (existingRoom == null)
      {
        return RedirectToAction("Index", "Admin");
      }

      // remove the program from the list of programs
      _context.Classrooms.Remove(existingRoom);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("Index", classroom);

    }
  }
}