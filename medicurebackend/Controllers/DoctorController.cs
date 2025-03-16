using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;

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
        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDoctor", new { id = doctor.DoctorID }, doctor);
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

        // DELETE: api/Doctor/5 - Delete a doctor by ID
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
        public IActionResult GetMyPatients()
        {
            // Logic to return patients assigned to the doctor
            return Ok(new { Message = "List of patients for the doctor" });
        }

        // Custom action: Create an appointment for a doctor (only Doctors can create)
        [HttpPost("create-appointment")]
        public IActionResult CreateAppointment([FromBody] Appointment appointment)
        {
            // Logic for creating an appointment (only Doctor can create)
            return Ok(new { Message = "Appointment created successfully!" });
        }

        // Helper method to check if the doctor exists
        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorID == id);
        }
    }
}
