using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;
using medicurebackend.Hubs;
using medicurebackend.Services;  // Import the namespace for EmailService

namespace medicurebackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly HospitalContext _context;
        private readonly EmailService _emailService;  // Inject EmailService

        public AppointmentController(HospitalContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;  // Assign EmailService
        }

        // GET: api/Appointment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _context.Appointments.ToListAsync();
        }

        // GET: api/Appointment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST: api/Appointment
        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAppointment(Appointment appointment)
        {
            // Ensure appointment is not scheduled in the past
            if (appointment.Date < DateTime.Today)
            {
                return BadRequest("Appointments cannot be scheduled in the past.");
            }

            // Check if the doctor is available at the requested time
            var existingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.DoctorID == appointment.DoctorID &&
                                          a.Date == appointment.Date &&
                                          a.Time == appointment.Time);

            if (existingAppointment != null)
            {
                return BadRequest("This doctor is already booked for the selected time.");
            }

            // Parse the Time if it's in string format (Ensure it's stored as TimeSpan)
            if (appointment.Time != null)
            {
                appointment.Time = TimeSpan.Parse(appointment.Time.ToString());  // Convert to TimeSpan if it's in string format
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Send email reminder after creating the appointment
            var message = $"Dear {appointment.PatientName},\n\nYou have an appointment scheduled with {appointment.DoctorName} on {appointment.AppointmentDate:MMMM dd, yyyy} at {appointment.AppointmentDate:HH:mm}.";
            await _emailService.SendEmailAsync(appointment.PatientEmail, "Appointment Reminder - MediCure Hospital", message);

            // Send a real-time notification for the appointment reminder
            var notificationHub = new NotificationHub();
            await notificationHub.SendAppointmentReminder(
                appointment.PatientName, appointment.AppointmentDate.ToString("MM/dd/yyyy HH:mm")
            );

            return CreatedAtAction("GetAppointment", new { id = appointment.AppointmentID }, appointment);
        }

        // PUT: api/Appointment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentID)
            {
                return BadRequest();
            }

            _context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                // Send an email when appointment status is updated
                var message = $"Appointment with {appointment.PatientName} has been marked as {appointment.Status}.";
                await _emailService.SendEmailAsync(appointment.PatientEmail, "Appointment Status Update - MediCure Hospital", message);

                // Send real-time notification for appointment status update
                var notificationHub = new NotificationHub();
                await notificationHub.SendAppointmentStatusUpdate(
                    $"Appointment with {appointment.PatientName} has been marked as {appointment.Status}"
                );
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Appointment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.AppointmentID == id);
        }

        // GET all appointments for a doctor
        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsForDoctor(int doctorId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.DoctorID == doctorId)
                .ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this doctor.");
            }

            return appointments;
        }

        // Get all appointments for a patient
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsForPatient(int patientId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.PatientID == patientId)
                .ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this patient.");
            }

            return appointments;
        }

        // GET recent appointments
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetRecentAppointments()
        {
            var recentAppointments = await _context.Appointments
                .OrderByDescending(a => a.AppointmentDate)
                .Take(5)  // Limit to 5 most recent appointments
                .Select(a => new
                {
                    a.AppointmentID,
                    PatientName = a.Patient.Name,  // Assuming there is a related Patient entity
                    DoctorName = a.Doctor.Name,    // Assuming there is a related Doctor entity
                    a.AppointmentDate,
                    a.Status
                })
                .ToListAsync();

            return Ok(recentAppointments);
        }
    }
}
