using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;

namespace medicurebackend.Controllers
{
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly HospitalContext _context;

        public PrescriptionController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Prescription/{patientId} - Get all prescriptions for a patient
        [HttpGet("{patientId}")]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescriptionsForPatient(int patientId)
        {
            var prescriptions = await _context.Prescriptions
                .Where(p => p.PatientID == patientId)
                .ToListAsync();

            if (prescriptions == null || !prescriptions.Any())
            {
                return NotFound("No prescriptions found for this patient.");
            }

            return prescriptions;
        }

        // POST: api/Prescription - Create a new prescription (only a doctor can create prescriptions)
        [HttpPost]
        public async Task<ActionResult<Prescription>> PostPrescription(Prescription prescription)
        {
            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrescriptionsForPatient), new { patientId = prescription.PatientID }, prescription);
        }
    }
}
