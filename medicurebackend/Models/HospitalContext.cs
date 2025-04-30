using Microsoft.EntityFrameworkCore;

namespace medicurebackend.Models
{
    public class HospitalContext : DbContext
    {
        // Constructor to pass the options to the base DbContext class
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
        }

        // DbSets for each entity that will be used in the application
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<DoctorAvailability> DoctorAvailability { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Alert> Alerts { get; set; }

       
        

        // OnModelCreating to set up additional configurations for the models
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Example: Configuring Billing entity's Amount property
            modelBuilder.Entity<Billing>()
                .Property(b => b.Amount)
                .HasColumnType("DECIMAL(18,2)");  // This ensures Amount has 18 digits, 2 decimal places

           

            

            
          


            

            base.OnModelCreating(modelBuilder);  // Always call base method
        }
    }
}
