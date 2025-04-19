using Microsoft.EntityFrameworkCore;

namespace medicurebackend.Models
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
        }

        // DbSets for the entities in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<Billing> Billings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring Billing entity's Amount property
            modelBuilder.Entity<Billing>()
                .Property(b => b.Amount)
                .HasColumnType("DECIMAL(18,2)"); // This will set the precision and scale for Amount (18 digits, 2 decimal places)

            base.OnModelCreating(modelBuilder);
        }
    }
}
