using Microsoft.AspNetCore.Identity;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Areas.Data
{
    //A class to store the user details
    public class PlatformUsers : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Cellphone { get; set; }
        public string Address { get; set; }
        public string CellPhoneNumber { get; internal set; }
    }
}
