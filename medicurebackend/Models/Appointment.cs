namespace medicurebackend.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }

        // Make sure to add the required properties
        public Patient? Patient { get; set; }  // Patient object
        public Doctor? Doctor { get; set; }    // Doctor object
      
        // Add missing properties
        public DateTime AppointmentDate { get; set; }  // Date of appointment
        public TimeSpan? AppointmentTime { get; set; }  // Time of appointment
        public string? Status { get; set; }  // Appointment status (e.g., Pending, Confirmed, etc.)
        public required string PatientName { get; set; }  // Patient's name
        public string? DoctorName { get; set; }   // Doctor's name
        public required string PatientEmail { get; set; } // Patient's email address
    }
}
