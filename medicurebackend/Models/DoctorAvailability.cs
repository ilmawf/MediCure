using System;

namespace medicurebackend.Models
{
    public class DoctorAvailability
    {
        public int DoctorAvailabilityID { get; set; }
        public int DoctorID { get; set; }
        public TimeSpan StartTime { get; set; }  // The start time of the availability slot
        public TimeSpan EndTime { get; set; }    // The end time of the availability slot
        public bool IsBooked { get; set; } = false; // Whether the time slot is already booked
    }
}
