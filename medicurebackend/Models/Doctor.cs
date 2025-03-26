namespace medicurebackend.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Specialty { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }  
        

    public Doctor()
    {
        
        Name = string.Empty; // Name cannot be null
    }
    }
}