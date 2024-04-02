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
        // Include other properties as needed
      })
      .OrderByDescending(p => p.Name)
      .ToList();

      // var technologies = _context.Technologies
      // .Select(t => new TechnologyModel
      // {
      //   Id = t.Id,
      //   Description = t.Description,
      //   // Include other properties as needed
      // })
      // .OrderByDescending(t => t.Description)
      // .ToList();

      // var users = _context.Users
      // .Select(u => new UserModel
      // {
      //   Id = u.Id,
      //   FirstName = u.FirstName,
      //   LastName = u.LastName,
      //   SetAsAdmin = u.SetAsAdmin,
      //   UserName = u.UserName,
      //   Password = u.Password,
      //   Salt = u.Salt,
      //   // Include other properties as needed
      // })
      // .OrderByDescending(u => u.UserName)
      // .ToList();

      var viewModel = new IndexViewModel
      {
        Programs = programs,
        // Technologies = technologies,
        // Users = users
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
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }
      var existingProgram = _context.Programs.Find(program.Id);
      if (existingProgram == null)
      {
        return RedirectToAction("Index", "Admin");
      }

      // remove the program from the list of programs
      _context.Programs.Remove(existingProgram);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("Index", program);

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