using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace medicurebackend.Controllers
{
    // Only Doctors can access this controller
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly HospitalContext _context;

        // Constructor to inject the DbContext
        public DoctorController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Doctor - Get all doctors (Admin or authorized users)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
        {
            return await _context.Doctors.ToListAsync();
        }

        // GET: api/Doctor/5 - Get a specific doctor by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
            {
                return NotFound();
            }

            return doctor;
        }

        // POST: api/Doctor - Create a new doctor (Admin role should handle)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctor), new { id = doctor.DoctorID }, doctor);
        }

        // PUT: api/Doctor/5 - Update an existing doctor's details
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.DoctorID)
            {
                return BadRequest();
            }

            _context.Entry(doctor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(id))
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

        // DELETE: api/Doctor/5 - Delete a doctor by ID (only Admin can delete)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Custom action: Get the patients assigned to this doctor
        [HttpGet("my-patients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetMyPatients()
        {
            var doctorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var patients = await _context.Patients
                .Where(p => p.DoctorID.ToString() == doctorId)
                .ToListAsync();

            if (patients == null || !patients.Any())
            {
                return NotFound("No patients found for this doctor.");
            }

            return Ok(patients);
        }

        // Custom action: Create an appointment for a doctor (only Doctors can create)
        [HttpPost("create-appointment")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] Appointment appointment)
        {
            var doctorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Only allow the doctor to create appointments for themselves
            if (appointment.DoctorID.ToString() != doctorId)
            {
                return Unauthorized("You can only create appointments for yourself.");
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateAppointment), new { id = appointment.AppointmentID }, appointment);
        }

        // Helper method to check if the doctor exists
        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorID == id);
        }

        // GET: api/Doctor/appointments - Get all appointments for the doctor
        [HttpGet("appointments")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetDoctorAppointments()
        {
            var doctorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var appointments = await _context.Appointments
                .Where(a => a.DoctorID.ToString() == doctorId)
                .ToListAsync();

            return appointments;
        }
    }
}
