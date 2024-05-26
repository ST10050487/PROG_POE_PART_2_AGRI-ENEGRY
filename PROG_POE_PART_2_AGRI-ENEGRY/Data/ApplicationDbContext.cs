using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PROG_POE_PART_2_AGRI_ENEGRY.Areas.Data;
using PROG_POE_PART_2_AGRI_ENEGRY.Models;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Data
{
    public class ApplicationDbContext : IdentityDbContext<PlatformUsers>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PROG_POE_PART_2_AGRI_ENEGRY.Models.Farmer> Farmer { get; set; } = default!;
        public DbSet<PROG_POE_PART_2_AGRI_ENEGRY.Models.EmployeeID> EMPLOYEE_IDS { get; set; } = default!;
        public DbSet<PROG_POE_PART_2_AGRI_ENEGRY.Models.Product> Product { get; set; } = default!;
        public DbSet<PROG_POE_PART_2_AGRI_ENEGRY.Models.Category> Categories { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
