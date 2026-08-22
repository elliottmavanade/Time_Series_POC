using System;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

public partial class TimeSeriesPocDbContext : DbContext
{
    public TimeSeriesPocDbContext(DbContextOptions<TimeSeriesPocDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ScheduledCalc> ScheduledCalcs { get; set; }

    public virtual DbSet<Sensor> Sensors { get; set; }

    public virtual DbSet<SensorCalcRelationship> SensorCalcRelationships { get; set; }

    public virtual DbSet<SensorCategory> SensorCategories { get; set; }

    public virtual DbSet<SensorReading> SensorReadings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScheduledCalc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Schedule__3214EC07DC2717F1");

            entity.HasOne(d => d.Sensor).WithMany(p => p.ScheduledCalcs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Scheduled__Senso__48CFD27E");
        });

        modelBuilder.Entity<Sensor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sensors__3214EC076CE4F569");

            entity.HasOne(d => d.SensorModel).WithMany(p => p.Sensors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sensors__Metadat__3F466844");
        });

        modelBuilder.Entity<SensorCalcRelationship>(entity =>
        {
            entity.HasKey(e => new { e.ParentSensorId, e.ChildSensorId }).HasName("PK__Sensor_C__86FF49B4241DE64E");

            entity.HasOne<Sensor>().WithMany()
                .HasForeignKey(e => e.ParentSensorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sensor_Ca__Senso__44FF419A");

            entity.HasOne<Sensor>().WithMany()
                .HasForeignKey(e => e.ChildSensorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sensor_Ca__Senso__45F365D3");
        });

        modelBuilder.Entity<SensorCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sensor_C__3214EC07A98D6D8B");
        });

        modelBuilder.Entity<SensorReading>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sensor_R__3214EC072F3E6F6B");

            entity.HasOne(d => d.Sensor).WithMany(p => p.SensorReadings).HasConstraintName("FK__Sensor_Re__Senso__4222D4EF");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
