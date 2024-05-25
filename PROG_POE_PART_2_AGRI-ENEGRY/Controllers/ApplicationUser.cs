using Microsoft.AspNetCore.Identity;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Controllers
{
    internal class ApplicationUser : IdentityUser
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}