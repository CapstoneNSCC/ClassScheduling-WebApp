using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using Microsoft.AspNetCore.Http;
using ClassScheduling_WebApp.Data;
using System.Security.Cryptography.X509Certificates;

namespace ClassScheduling_WebApp.Controllers
{
  public class LoginController : Controller
  {
    private readonly ApplicationDbContext _context;

    public LoginController(ApplicationDbContext context)
    {
      _context = context;
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Submit(string myUsername, string myPassword)
    {
      WebLogin webLogin = new WebLogin(_context, HttpContext)
      {
        //update properties
        Username = myUsername,
        Password = myPassword
      };

      if (webLogin.Unlock())
      {
        if (HttpContext.Session.GetString("admin") == "true")
        {
          return RedirectToAction("Index", "Admin");
        }
        return RedirectToAction("HomeIndex", "Home");
      }
      else
      {
        ViewData["feedback"] = "Incorrect Username and/or Password. Please try again...";
        return View("Index");
      }
    }

    public IActionResult UserIndex()
    {
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var users = _context.Users
      .Select(u => new UserModel
      {
        Id = u.Id,
        FirstName = u.FirstName,
        LastName = u.LastName,
        SetAsAdmin = u.SetAsAdmin,
        UserName = u.UserName,
        Password = u.Password,
        Salt = u.Salt,
      })
      .OrderByDescending(u => u.UserName)
      .ToList();

      var viewModel = new IndexViewModel
      {
        Users = users
      };
      return View(viewModel);
    }

    public IActionResult AddUser()
    {
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Admin");
      }
      return View();
    }

    public IActionResult AddSubmit(UserModel user)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Admin");
      }
      //check if username is already taken
      var Userame = user.UserName;
      var existingUser = _context.Users.FirstOrDefault(u => u.UserName == Userame);
      if (existingUser != null)
      {
        ViewData["feedback"] = "User already exists, Please add a different user.";
        return View("AddUser", user);
      }
      var salt = user.getSalt();
      user.Salt = salt;

      var hashedPassword = user.GetHashed(user.Password, salt);
      user.Password = hashedPassword;

      // add the program to the list of programs
      _context.Users.Add(user);
      //save changes to the database
      _context.SaveChanges();

      return RedirectToAction("UserIndex", user);
    }

    [Route("/Login/Update/{UserID:int}")]
    public IActionResult Update(int UserID)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // find the technology by the technologyID
      UserModel user = _context.Users.Find(UserID);
      //passing in technology model to the view
      return View("EditUser", user);
    }

    public IActionResult UpdateSubmit(UserModel user)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Login");
      }


      var salt = user.getSalt();
      user.Salt = salt;

      var hashedPassword = user.GetHashed(user.Password, salt);
      user.Password = hashedPassword;

      // update the program in the list of programs
      _context.Users.Update(user);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("UserIndex", user);
    }

    [Route("/Login/Delete/{UserID:int}")]
    public IActionResult Delete(int UserID)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      // find the user by the userID
      UserModel user = _context.Users.Find(UserID);

      return View("DeleteUser", user);

    }

    public IActionResult DeleteSubmit(UserModel user)
    {
      // if auth is not  = true, it re-directs to the login screen.
      if (HttpContext.Session.GetString("admin") != "true")
      {
        return RedirectToAction("Index", "Login");
      }

      var existingUser = _context.Users.Find(user.Id);
      if (existingUser == null)
      {
        return RedirectToAction("Index", "Admin");
      }
      // remove the user
      _context.Users.Remove(existingUser);
      //save changes to the database
      _context.SaveChanges();
      return RedirectToAction("UserIndex", user);

    }
  }
}
