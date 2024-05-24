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
    }
}
