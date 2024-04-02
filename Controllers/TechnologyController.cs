using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using System.Collections.Immutable;
using ClassScheduling_WebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ClassScheduling_WebApp.Controllers
{
  public class TechnologyController : Controller
  {

    private readonly ApplicationDbContext _context;

    public TechnologyController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var technologies = _context.Technologies
      .Select(t => new TechnologyModel
      {
        Id = t.Id,
        Description = t.Description,
        // Include other properties as needed
      })
      .OrderByDescending(t => t.Description)
      .ToList();

      var viewModel = new IndexViewModel
      {
        Technologies = technologies
      };

      return View(viewModel);
    }


    public IActionResult AddTech()
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // construct course object that will be used to add a new course.
      TechnologyModel tech = new TechnologyModel
      {
        Description = "",
      };
      //passing in technology model to the view
      return View(tech);
    }

    public IActionResult AddSubmit(TechnologyModel tech)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // add the technology to the list of technologies
      _context.Technologies.Add(tech);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("Index", "Admin");
    }


    [Route("/Technology/Update/{technologyID:int}")]
    public IActionResult Update(int technologyID)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // find the technology by the technologyID
      TechnologyModel tech = _context.Technologies.Find(technologyID);
      //passing in technology model to the view
      return View("EditTech", tech);
    }


    public IActionResult UpdateSubmit(TechnologyModel tech)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // update the program in the list of programs
      _context.Technologies.Update(tech);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("Index", "Admin");
    }

    [Route("/Technology/Delete/{TechnologyID:int}")]
    public IActionResult Delete(int technologyID)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // find the technology by the technologyID
      TechnologyModel tech = _context.Technologies.Find(technologyID);

      return View("DeleteTech", tech);

    }

    public IActionResult DeleteSubmit(TechnologyModel tech)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("auth") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var existingTech = _context.Technologies.Find(tech.Id);
      if (existingTech == null)
      {
        return RedirectToAction("Index", "Admin");
      }

      // remove the program from the list of programs
      _context.Technologies.Remove(existingTech);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("Index", "Admin");

    }


  }
}