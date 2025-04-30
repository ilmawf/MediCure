namespace medicurebackend.Models
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        
        // Add the missing properties
        public string? PrescriptionDetails { get; set; }  // Prescription information
        public DateTime RequestedAt { get; set; }         // Prescription request date

        // Assuming you have Patient and Doctor relationships
        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}
