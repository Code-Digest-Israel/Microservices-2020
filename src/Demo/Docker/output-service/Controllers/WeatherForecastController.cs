using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace output_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(ILogger<NotificationsController> logger)
        {
            Console.WriteLine("hi1");

            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<OutrageNotification> Get()
        {
            Console.WriteLine("hi");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new OutrageNotification
            {
                Date = DateTime.Now.AddDays(index),
                NumOfPatients = rng.Next(-20, 55),
                City = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult PostUpdate(OutrageNotification outrageNotification)
        {
            Console.WriteLine("hi post");
            //send SignalR notification
            return Ok(outrageNotification);
        }
    }
}
