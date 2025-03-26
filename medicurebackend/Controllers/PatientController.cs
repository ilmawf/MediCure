using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;

namespace medicurebackend.Controllers
{
    // Only Patients can access this controller
    [Authorize(Roles = "Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly HospitalContext _context;

        // Constructor to inject the DbContext
        public PatientController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Patient - Fetch all patients (this can be restricted to Admins only)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        // GET: api/Patient/5 - Get details for a specific patient by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // POST: api/Patient - Create a new patient (should be handled by Admin, not Patient)
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.PatientID }, patient);
        }

        // PUT: api/Patient/5 - Update a patient's information (only the Patient themselves should update their record)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // DELETE: api/Patient/5 - Delete a patient's record (should be restricted to Admins)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
        }
    }
}
