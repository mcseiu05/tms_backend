using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TMS.Common.Model;

namespace TMS.DAL.Model
{
    public partial class TMSContext : DbContext
    {
        public TMSContext()
        {
        }

        public TMSContext(DbContextOptions<TMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LoggedinStatus> LoggedinStatus { get; set; }
        public virtual DbSet<LoginInfo> LoginInfo { get; set; }
        public virtual DbSet<Tasks> Task { get; set; }
        public virtual DbSet<TaskPriority> TaskPriority { get; set; }
        public virtual DbSet<TaskStatus> TaskStatus { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-NU80LQG\\SQLSERVER;Database=TMS;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoggedinStatus>(entity =>
            {
                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<LoginInfo>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.LoggedOutTime).HasColumnType("datetime");

                entity.Property(e => e.LoginTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.AssignedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(150);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.TaskName).HasMaxLength(100);
            });

            modelBuilder.Entity<TaskPriority>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.PriorityLevel).HasMaxLength(50);
            });

            modelBuilder.Entity<TaskStatus>(entity =>
            {
                entity.Property(e => e.StatusName).HasMaxLength(50);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(80);

                entity.Property(e => e.LastName).HasMaxLength(80);

                entity.Property(e => e.MobileNo).HasMaxLength(13);

                entity.Property(e => e.Password).HasMaxLength(100);

                entity.Property(e => e.RegistrationDate).HasColumnType("datetime");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
