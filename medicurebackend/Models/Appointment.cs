namespace medicurebackend.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string? Status { get; set; }

        public Patient Patient {get; set;}
        public Doctor Doctor {get; set;}
    }
}