using System;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DAL
{
    public partial class StreetManagementContext : DbContext
    {
        public StreetManagementContext()
        {
        }

        public StreetManagementContext(DbContextOptions<StreetManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Camera> Cameras { get; set; }
        public virtual DbSet<Lamp> Lamps { get; set; }
        public virtual DbSet<LampType> LampTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Ukrainian_CI_AS");

            modelBuilder.Entity<Camera>(entity =>
            {
                entity.ToTable("Camera");

                entity.HasKey(entity => entity.Id);

                entity.Property(entity => entity.Id).ValueGeneratedOnAdd();

                entity.HasIndex(e => e.Photo, "UQ__Camera__5C7E46C8822F2A84")
                    .IsUnique();

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.Photo);
            });

            modelBuilder.Entity<Lamp>(entity =>
            {
                entity.ToTable("Lamp");

                entity.HasKey(entity => entity.Id);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.HasOne(d => d.Camera)
                    .WithMany(p => p.Lamps)
                    .HasForeignKey(d => d.CameraId)
                    .HasConstraintName("FK_Lamp_CameraId_Camera_CameraId");

                entity.HasOne(d => d.LampType)
                    .WithMany(p => p.Lamps)
                    .HasForeignKey(d => d.LampTypeId)
                    .HasConstraintName("FK_Lamp_LampTypeId_LampType_LampTypeId");
            });

            modelBuilder.Entity<LampType>(entity =>
            {
                entity.ToTable("LampType");

                entity.HasKey(entity => entity.Id);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
