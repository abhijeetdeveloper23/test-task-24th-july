using Microsoft.EntityFrameworkCore;
using ModuleImplementation.Models;

namespace ModuleImplementation.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Reminder> Reminder { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships, constraints, etc. if needed
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();

                entity.HasMany(d => d.SubDepartments)
                      .WithOne(sd => sd.ParentDepartment)
                      .HasForeignKey(sd => sd.ParentDepartmentId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
