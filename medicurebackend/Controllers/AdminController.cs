using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;

namespace medicurebackend.Controllers
{
    // Only Admins can access this controller
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly HospitalContext _context;

        public AdminController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/Admin/get-users
        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            // Logic to return all users (Admin, Doctor, Patient, etc.)
            var users = await _context.Users.ToListAsync();  // Fetch all users from the Users table
            if (users == null || !users.Any())
            {
                return NotFound(new { Message = "No users found." });
            }

            return Ok(users);  // Return the list of users
        }

        // POST: api/Admin/create-doctor
        [HttpPost("create-doctor")]
        public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
        {
            if (doctor == null)
            {
                return BadRequest(new { Message = "Invalid doctor data." });
            }

            // You can add validation or additional logic here to check for duplicate doctors or assign default settings

            // Add the new doctor to the database
            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();  // Save the doctor to the database

            return Ok(new { Message = "Doctor created successfully!" });
        }

        // PUT: api/Admin/update-doctor/{id}
        [HttpPut("update-doctor/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (doctor == null || doctor.DoctorID != id)
            {
                return BadRequest(new { Message = "Invalid doctor data or mismatched ID." });
            }

            // Find the doctor to update
            var existingDoctor = await _context.Doctors.FindAsync(id);
            if (existingDoctor == null)
            {
                return NotFound(new { Message = "Doctor not found." });
            }

            // Update the doctor details
            existingDoctor.Name = doctor.Name;
            existingDoctor.Specialty = doctor.Specialty;
            existingDoctor.ContactNumber = doctor.ContactNumber;
            existingDoctor.Email = doctor.Email;

            await _context.SaveChangesAsync();  // Save updated doctor to the database

            return Ok(new { Message = "Doctor updated successfully!" });
        }

        // DELETE: api/Admin/delete-doctor/{id}
        [HttpDelete("delete-doctor/{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found." });
            }

            // Remove the doctor from the database
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();  // Save changes to the database

            return Ok(new { Message = "Doctor deleted successfully!" });
        }
    }
}
