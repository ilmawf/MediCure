using System;

namespace medicurebackend.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }  // Roles like Doctor, Nurse, Receptionist
        public string? Department { get; set; }  // Department the staff belongs to
        public string? DutyTime { get; set; }  // Duty Time for the staff (e.g., Morning, Night)
    }
}
