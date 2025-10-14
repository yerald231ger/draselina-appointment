using drselina.appointment.api.Features.Appointments;
using Microsoft.EntityFrameworkCore;

namespace drselina.appointment.api.Data;

public class AppointmentDbContext : DbContext
{
    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
    {
    }

    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PatientName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PatientEmail).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PatientPhone).IsRequired().HasMaxLength(50);
            entity.Property(e => e.AppointmentDate).IsRequired();
            entity.Property(e => e.Reason).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}
