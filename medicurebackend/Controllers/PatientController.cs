using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;
using medicurebackend.Hubs;
using medicurebackend.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace medicurebackend.Controllers
{
    [Authorize(Roles = "Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly HospitalContext _context;
        private readonly EmailService _emailService;

        public PatientController(HospitalContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        // Get details for a specific patient by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(patientId) || patient.PatientID != int.Parse(patientId))
            {
                return Forbid();
            }

            return patient;
        }

        // Create a new patient (restricted to Admin only)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var notificationHub = new NotificationHub();
            if (!string.IsNullOrEmpty(patient.Name))
            {
                await notificationHub.SendPatientRegistrationNotification(patient.Name);
            }

            if (!string.IsNullOrEmpty(patient.Email))
            {
                var message = $"Dear {patient.Name},\n\nYou have been successfully registered at MediCure Hospital.\n\nWelcome!";
                await _emailService.SendEmailAsync(patient.Email, "Patient Registration - MediCure Hospital", message);
            }

            return CreatedAtAction("GetPatient", new { id = patient.PatientID }, patient);
        }

        // Update a patient's information
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.PatientID)
            {
                return BadRequest();
            }

            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(patientId) || patient.PatientID != int.Parse(patientId))
            {
                return Forbid();
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

        // Delete a patient's record (restricted to Admins only)
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

        // Get the patients assigned to this doctor
        [HttpGet("my-patients")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetMyPatients()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
            {
                return BadRequest("Doctor ID not found.");
            }

            var patients = await _context.Patients
                .Where(p => p.DoctorID.ToString() == doctorId)
                .ToListAsync();

            if (patients == null || !patients.Any())
            {
                return NotFound("No patients found for this doctor.");
            }

            return Ok(patients);
        }

        // Create an appointment for a doctor
        [HttpPost("create-appointment")]
        public async Task<ActionResult<Appointment>> CreateAppointment([FromBody] Appointment appointment)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (appointment.DoctorID.ToString() != doctorId)
            {
                return Unauthorized("You can only create appointments for yourself.");
            }

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateAppointment), new { id = appointment.AppointmentID }, appointment);
        }

        // Get all appointments for the patient
        [HttpGet("appointments")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetPatientAppointments()
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Patient ID not found.");
            }

            var appointments = await _context.Appointments
                .Where(a => a.PatientID.ToString() == patientId)
                .ToListAsync();

            return appointments;
        }

        // Request a prescription (patient makes a request for a prescription)
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

        // Update patient information
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

        // Helper method to check if the patient exists
        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
        }
    }
}
