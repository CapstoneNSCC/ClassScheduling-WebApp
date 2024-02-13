using System;
using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Models;
using Microsoft.AspNetCore.Http;

namespace userAuthentication.Controllers
{

  public class LoginController : Controller
  {

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Submit(string myUsername, string myPassword)
    {
      WebLogin webLogin = new WebLogin(Connection.CONNECTION_STRING, HttpContext);

      //update properties

      webLogin.username = myUsername;
      webLogin.password = myPassword;

      //attempt unlock
      if (webLogin.unlock())
      {
        return RedirectToAction("Index", "Admin");
      }
      else
      {
        //incorrect login - update feedback
        ViewData["feedback"] = "Incorrect Username and/or Password. Please try again...";
      }

      return View("Index");
    }
  }
}
