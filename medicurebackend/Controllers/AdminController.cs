using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;

namespace medicurebackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly HospitalContext _context;

        public AdminController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/admin/metrics - Fetch dashboard metrics (total patients, appointments today, total staff)
        [HttpGet("metrics")]
        public async Task<ActionResult> GetDashboardMetrics()
        {
            // Total Patients
            var totalPatients = await _context.Patients.CountAsync();

            // Appointments Today (you can filter by today's date)
            var todayAppointments = await _context.Appointments
                .Where(a => a.AppointmentDate.Date == DateTime.Now.Date)
                .CountAsync();

            // Total Staff (Doctors or users with staff role)
            var totalStaff = await _context.Users
                .Where(u => u.Role == "staff")
                .CountAsync();

            // Optional: Lab Reports (you can add logic for lab reports if needed)
            var labReportsCount = await _context.Medications.CountAsync(); // Mocking lab reports with Medications count

            var metrics = new
            {
                TotalPatients = totalPatients,
                AppointmentsToday = todayAppointments,
                TotalStaff = totalStaff,
                LabReportsCount = labReportsCount
            };

            return Ok(metrics);
        }

        // AdminController.cs
[HttpGet("appointments-stats")]
public async Task<ActionResult> GetAppointmentsStats()
{
    var startDate = DateTime.Now.AddDays(-7); // Get data for the last 7 days
    var appointmentsStats = await _context.Appointments
        .Where(a => a.AppointmentDate >= startDate)
        .GroupBy(a => a.AppointmentDate.Date)
        .Select(g => new
        {
            Date = g.Key,
            Count = g.Count()
        })
        .ToListAsync();

    // Prepare data for Chart.js
    var dates = appointmentsStats.Select(a => a.Date.ToString("yyyy-MM-dd")).ToArray();
    var appointments = appointmentsStats.Select(a => a.Count).ToArray();

    return Ok(new { dates, appointments });
}

[HttpGet("alerts")]
public IActionResult GetAlerts()
{
    var alerts = new List<object>
    {
        new { title = "Low Stock", message = "Pharmacy is running low on medications." },
        new { title = "High Load", message = "Server load is very high. Check performance." }
    };

    return Ok(alerts);
}
    }
}
