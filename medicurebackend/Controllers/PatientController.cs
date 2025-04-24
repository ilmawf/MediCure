using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;
using medicurebackend.Hubs;
using medicurebackend.Services;  // Import the EmailService namespace

namespace medicurebackend.Controllers
{
    // Only Patients can access this controller by default
    [Authorize(Roles = "Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly HospitalContext _context;
        private readonly EmailService _emailService;  // Inject EmailService

        // Constructor to inject the DbContext and EmailService
        public PatientController(HospitalContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;  // Assign EmailService
        }

        // GET: api/Patient - Fetch all patients (restricted to Admins only)
        [Authorize(Roles = "Admin")]
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

            // Ensure that a patient can only view their own data
            if (User.IsInRole("Patient") && patient.PatientID != int.Parse(User.Identity.Name))
            {
                return Forbid();  // If a patient tries to view another patient's data
            }

            return patient;
        }

        // POST: api/Patient - Create a new patient (restricted to Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Send a notification when a new patient is added
            var notificationHub = new NotificationHub();
            await notificationHub.SendPatientRegistrationNotification(patient.Name);

            // Send a confirmation email to the new patient
            var message = $"Dear {patient.Name},\n\nYou have been successfully registered at MediCure Hospital.\n\nWelcome!";
            await _emailService.SendEmailAsync(patient.Email, "Patient Registration - MediCure Hospital", message);

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

            // Ensure that patients can only update their own record
            if (User.IsInRole("Patient") && patient.PatientID != int.Parse(User.Identity.Name))
            {
                return Forbid();  // If a patient tries to update another patient's data
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

        // DELETE: api/Patient/5 - Delete a patient's record (restricted to Admins only)
        [Authorize(Roles = "Admin")]
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

        // GET: api/Patient?search=name&doctorId=3 - Get filtered patient list
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients([FromQuery] string search = "", [FromQuery] int? doctorId = null)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Email.Contains(search));
            }

            if (doctorId.HasValue)
            {
                query = query.Where(p => p.DoctorID == doctorId.Value);
            }

            var patients = await query.ToListAsync();

            return Ok(patients);
        }

        // POST: api/patient - Create a new patient (including doctor/ward assignment)
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Send a notification when a new patient is added
            var notificationHub = new NotificationHub();
            await notificationHub.SendPatientRegistrationNotification(patient.Name);

            // Send email confirmation to the patient
            var message = $"Dear {patient.Name},\n\nYou have been successfully registered at MediCure Hospital.\n\nWelcome!";
            await _emailService.SendEmailAsync(patient.Email, "Patient Registration - MediCure Hospital", message);

            return CreatedAtAction("GetPatient", new { id = patient.PatientID }, patient);
        }

        // PUT: api/patient/5 - Update a patient's details (including doctor/ward assignment)
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


        [Authorize(Roles = "Patient")]
[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly HospitalContext _context;

    public PatientController(HospitalContext context)
    {
        _context = context;
    }

    // GET: api/Patient/appointments
    [HttpGet("appointments")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetPatientAppointments()
    {
        var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var appointments = await _context.Appointments
            .Where(a => a.PatientID == patientId)
            .ToListAsync();
        
        return appointments;
    }
}
[HttpPost("request-prescription")]
public async Task<IActionResult> RequestPrescription([FromBody] PrescriptionRequest request)
{
    var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var patient = await _context.Patients.FindAsync(patientId);

    if (patient == null)
    {
        return NotFound();
    }

    var prescription = new Prescription
    {
        PatientID = patient.PatientID,
        DoctorID = request.DoctorID,
        PrescriptionDetails = request.PrescriptionDetails,
        RequestedAt = DateTime.Now,
    };

    _context.Prescriptions.Add(prescription);
    await _context.SaveChangesAsync();

    return Ok(new { Message = "Prescription request sent successfully!" });
}

[HttpPut("update-info")]
public async Task<IActionResult> UpdatePatientInfo([FromBody] UpdatePatientInfoRequest request)
{
    var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var patient = await _context.Patients.FindAsync(patientId);

    if (patient == null)
    {
        return NotFound();
    }

    patient.Email = request.Email ?? patient.Email;
    patient.PhoneNumber = request.PhoneNumber ?? patient.PhoneNumber;
    patient.MedicalHistory = request.MedicalHistory ?? patient.MedicalHistory;

    _context.Entry(patient).State = EntityState.Modified;
    await _context.SaveChangesAsync();

    return Ok(new { Message = "Patient information updated successfully!" });
}

    }
}
