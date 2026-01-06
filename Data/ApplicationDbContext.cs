using BiartBiPortal.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore; // ModelBuilder için bu using gerekli olabilir

namespace BiartBiPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Domain DbSets
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportCategory> ReportCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Bu satır daima en üstte kalmalı

            // Many-to-Many: Report <-> Category
            builder.Entity<ReportCategory>()
                .HasKey(rc => new { rc.ReportId, rc.CategoryId });

            builder.Entity<ReportCategory>()
                .HasOne(rc => rc.Report)
                .WithMany(r => r.ReportCategories)
                .HasForeignKey(rc => rc.ReportId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReportCategory>()
                .HasOne(rc => rc.Category)
                .WithMany(c => c.ReportCategories)
                .HasForeignKey(rc => rc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-Many: User -> Reports
            builder.Entity<Report>()
                .HasOne(r => r.CreatedByUser)
                .WithMany()
                .HasForeignKey(r => r.CreatedByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            // SQLite 'nvarchar(max)' desteklemediği için Identity tablolarını
            // ve diğer potansiyel string'leri manuel olarak yeniden yapılandırıyoruz.
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        // MaxLength belirtilmemiş string'leri (SQL Server'da nvarchar(max) olanları)
                        // SQLite'ın anlayacağı 'TEXT' tipine dönüştür.
                        property.SetColumnType("TEXT");
                    }
                }
            }
        }
    }
}