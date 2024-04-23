using Microsoft.EntityFrameworkCore;
using Permissions.Infrastructure.Models;

namespace Permissions.Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
            var ptype = this.PermissionTypes.FirstOrDefault(p => p.Id == 1);
            if (ptype == null) {
                this.PermissionTypes.Add(new PermissionType { Description = "Escritura" });
            }
            this.SaveChanges();
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PermissionType> PermissionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla de permisos
            modelBuilder.Entity<Permission>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Permission>()
                .Property(p => p.EmployeeForename)
                .IsRequired();
            modelBuilder.Entity<Permission>()
                .Property(p => p.EmployeeSurname)
                .IsRequired();
            modelBuilder.Entity<Permission>()
                .Property(p => p.PermissionDate)
                .IsRequired();

            // Configuración de la relación entre Permission y PermissionType
            modelBuilder.Entity<Permission>()
                .HasOne(p => p.PermissionType)
                .WithMany(pt => pt.Permissions)
                .HasForeignKey(p => p.PermissionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de la tabla de tipos de permisos
            modelBuilder.Entity<PermissionType>()
                .HasKey(pt => pt.Id);
            modelBuilder.Entity<PermissionType>()
                .Property(pt => pt.Description)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
