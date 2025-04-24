using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using medicurebackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace medicurebackend.Controllers
{
    [Authorize(Roles = "Admin, Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly HospitalContext _context;

        public DoctorAvailabilityController(HospitalContext context)
        {
            _context = context;
        }

        // GET: api/DoctorAvailability - Get availability for a doctor
        [HttpGet("{doctorId}")]
        public async Task<ActionResult<IEnumerable<DoctorAvailability>>> GetDoctorAvailability(int doctorId)
        {
            var availability = await _context.DoctorAvailability
                .Where(a => a.DoctorID == doctorId && a.IsBooked == false)
                .ToListAsync();

            if (availability == null || !availability.Any())
            {
                return NotFound("No available time slots found.");
            }

            return availability;
        }

        // POST: api/DoctorAvailability - Add new availability slots for a doctor
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<DoctorAvailability>> PostDoctorAvailability(DoctorAvailability availability)
        {
            _context.DoctorAvailability.Add(availability);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoctorAvailability), new { doctorId = availability.DoctorID }, availability);
        }

        // PUT: api/DoctorAvailability/5 - Mark a time slot as booked
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctorAvailability(int id, DoctorAvailability availability)
        {
            if (id != availability.DoctorAvailabilityID)
            {
                return BadRequest();
            }

            var existingAvailability = await _context.DoctorAvailability.FindAsync(id);
            if (existingAvailability == null)
            {
                return NotFound();
            }

            existingAvailability.IsBooked = true;  // Mark as booked

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
    }
}
