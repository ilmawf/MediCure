using Microsoft.AspNetCore.Mvc;
using medicurebackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace medicurebackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        private static List<Alert> _alerts = new List<Alert>
        {
            // Sample alerts for demonstration purposes
            new Alert { AlertID = 1, Title = "Low Stock", Message = "Pharmacy stock is running low!", Date = DateTime.Now.AddDays(-1), IsRead = false },
            new Alert { AlertID = 2, Title = "Appointment Reminder", Message = "You have an appointment tomorrow at 10 AM.", Date = DateTime.Now.AddDays(-1), IsRead = false }
        };

        // GET: api/alerts - Get all alerts
        [HttpGet]
        public ActionResult<IEnumerable<Alert>> GetAlerts()
        {
            return Ok(_alerts);  // Return the list of alerts
        }

        // POST: api/alerts - Create a new alert (e.g., triggered by an event in the system)
        [HttpPost]
        public ActionResult<Alert> CreateAlert([FromBody] Alert alert)
        {
            alert.AlertID = _alerts.Count + 1; // Assign a new ID
            alert.Date = DateTime.Now; // Set the current time as the date
            _alerts.Add(alert);  // Add the new alert to the list
            return CreatedAtAction(nameof(GetAlerts), new { id = alert.AlertID }, alert); // Return the created alert
        }

        // PUT: api/alerts/5 - Mark an alert as read
        [HttpPut("{id}")]
        public IActionResult MarkAsRead(int id)
        {
            var alert = _alerts.FirstOrDefault(a => a.AlertID == id);
            if (alert == null)
            {
                return NotFound();
            }

            alert.IsRead = true; // Mark as read
            return NoContent();  // Success response
        }
    }
}
