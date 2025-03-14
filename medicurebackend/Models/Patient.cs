namespace medicurebackend.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Contact { get; set; }
        public string MedicalHistory { get; set; }
    }
}