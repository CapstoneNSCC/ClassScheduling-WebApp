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
        public IActionResult AddRoom()
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            // construct course object that will be used to add a new course.
            ClassroomModel classroom = new ClassroomModel
            {
                RoomNumber = 0,
                BuildingAcronym = "",
            };
            //passing in technology model to the view
            return View(classroom);
        }

        public IActionResult AddSubmit(ClassroomModel classroom)
        {
            // if auth is not  = true, it re-directs to the login screen.
            if (HttpContext.Session.GetString("auth") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            // add the technology to the list of technologies
            _context.Classrooms.Add(classroom);
            //save changes to the database
            _context.SaveChanges();
            return RedirectToAction("Index", "Admin");
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
            return RedirectToAction("Index", "Admin");
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
            return RedirectToAction("Index", "Admin");

        }
    }
}