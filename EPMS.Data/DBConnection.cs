using EmployeePerformancesManagementSystem.MVC.Models;
using EmployeePerformancesManagementSystem.MVC.Models.EmployeeReviewSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace EPMS.Data
{
    public partial class DBConnection : DbContext
    {
        public DBConnection(DbContextOptions<DBConnection> options)
            : base(options)
        {
        }

        public DBConnection()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<PerformanceReport> PerformanceReports { get; set; }
        public DbSet<PerformanceCriteria> PerformanceCriteria { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<EmployeePerformance> EmployeePerformances { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=MOULITHARAN-LR0\\SQLEXPRESS1;Database=EmployeePerformanceManagementSystem;User Id=sa;Password=Password@123;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Goals relation
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.Employee)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedback relations
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Employee)
                .WithMany(u => u.FeedbackReceived)
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Reviewer)
                .WithMany(u => u.FeedbackGiven)
                .HasForeignKey(f => f.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notifications relation
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Performance Report Relations - Fix Multiple Cascade Path Issue
            modelBuilder.Entity<PerformanceReport>()
                .HasOne(p => p.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade); // ✅ Cascade only for EmployeeId

            modelBuilder.Entity<PerformanceReport>()
                .HasOne(p => p.HRAdmin)
                .WithMany()
                .HasForeignKey(p => p.HRAdminId)
                .OnDelete(DeleteBehavior.Restrict); // ✅ Restrict to prevent multiple cascade paths

            // Evaluation Relations
            modelBuilder.Entity<Evaluation>()
                .HasOne(e => e.Manager)
                .WithMany() // If Manager does not have a navigation property
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
