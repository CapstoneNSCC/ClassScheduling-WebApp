using Microsoft.AspNetCore.Mvc;
using ClassScheduling_WebApp.Data;
using ClassScheduling_WebApp.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Humanizer;

namespace ClassScheduling_WebApp.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Events
        // [HttpGet]
        // [Route("api/Events")]
        // public IAsyncResult GetEvents()
        // {
        //     //     var events =
        //     //     [
        //     // {
        //     //     title: 'INFT 3000',
        //     //   description: 'Capstone',
        //     //   daysOfWeek: ['1'],
        //     //   startTime: '10:30:00',
        //     //   endTime: '12:30:00',
        //     //   teacher: 'Ryan McLaren',
        //     //   classroom: 'TRF-308'
        //     // },
        //     // {
        //     //     title: 'COMM 4700',
        //     //   description: 'Professional Practice',
        //     //   daysOfWeek: ['1'],
        //     //   startTime: '10:30:00',
        //     //   endTime: '12:30:00',
        //     //   teacher: 'Nasser Jaleel',
        //     //   classroom: 'TRM-114',
        //     // },
        //     // {
        //     //     title: 'INFT 4000',
        //     //   description: 'Special Topics',
        //     //   daysOfWeek: ['1'],
        //     //   startTime: '10:30:00',
        //     //   endTime: '12:30:00',
        //     //   teacher: 'Sean Morrow',
        //     //   classroom: 'TRF-312',
        //     // },
        //     // {
        //     //     title: 'NWTS 1000',
        //     //   description: 'Networking and Security',
        //     //   daysOfWeek: ['1'],
        //     //   startTime: '10:30:00',
        //     //   endTime: '12:30:00',
        //     //   teacher: 'Gordon Larusic',
        //     //   classroom: 'TRF-306',
        //     // },
        //     // {
        //     //     title: 'WEBD 3027',
        //     //   description: 'Developing for CM Systems',
        //     //   daysOfWeek: ['4'],
        //     //   startTime: '13:30:00',
        //     //   endTime: '15:30:00',
        //     //   teacher: 'Ryan McLaren',
        //     //   classroom: 'TRF-312',
        //     // }
        //     // ];
        //     return events;
        // }
    }

}