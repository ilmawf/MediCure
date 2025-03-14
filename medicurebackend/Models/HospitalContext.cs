using Microsoft.EntityFrameworkCore;

namespace medicurebackend.Models
{
    public class HospitalContext(DbContextOptions<HospitalContext> options) : DbContext(options)
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Billing> Billings { get; set; }
    }
}
