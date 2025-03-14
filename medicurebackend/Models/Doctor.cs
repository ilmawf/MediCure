namespace medicurebackend.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public string Contact { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}