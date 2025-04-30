namespace medicurebackend.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? MedicalHistory { get; set; }
         public int DoctorID { get; set; }  
        public string? Ward { get; set; } 
        public string? Department { get; set; }

    }
}