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
        [Route("api/Events")]
        public JsonResult GetEvents()
        {
            var events = _context.Events.Select(e => new
            {
                id = e.Id,
                title = e.title,
                description = e.description,
                startTime = e.StartTime,
                endTime = e.EndTime,
                classroom = e.Classroom,
                teacher = e.teacher,
            })
                      .ToList();
            Console.WriteLine(events);


            return new JsonResult(events);
        }
    }

}