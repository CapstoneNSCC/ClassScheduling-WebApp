using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Immutable;
using ClassScheduling_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClassScheduling_WebApp.Controllers
{

  public class ProgramsController : Controller
  {
    private readonly ApplicationDbContext _context;

    public ProgramsController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var programs = _context.Programs
      .Select(p => new ProgramModel
      {
        Id = p.Id,
        Name = p.Name,
        Year = p.Year,
      })
      .OrderByDescending(p => p.Name)
      .ToList();

      var viewModel = new IndexViewModel
      {
        Programs = programs,
      };

      return View(viewModel);
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
      return RedirectToAction("Index", program);
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
      return RedirectToAction("Index", program);
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
      // if auth is not = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var existingProgram = _context.Programs
                                  .Include(p => p.Courses) // Include courses associated with the program
                                  .FirstOrDefault(p => p.Id == program.Id);

      if (existingProgram == null)
      {
        return RedirectToAction("Index", "Admin");
      }

      // Delete all courses associated with the program
      _context.Courses.RemoveRange(existingProgram.Courses);

      // Remove the program from the list of programs
      _context.Programs.Remove(existingProgram);

      // Save changes to the database
      _context.SaveChanges();

      return RedirectToAction("Index", "Admin");
    }

    [Route("/Program/ProgramDetails/{programID:int}")]
    public IActionResult ProgramDetails(int programID)
    {
      ProgramModel program = _context.Programs
      .Include(p => p.Courses)
      .ThenInclude(tc => tc.TechClasses)
      .ThenInclude(tcc => tcc.Technology)
      .FirstOrDefault(p => p.Id == programID);
      return View("SingleProgram", program);
    }
  }
}