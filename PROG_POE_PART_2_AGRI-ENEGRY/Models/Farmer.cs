using System.ComponentModel.DataAnnotations;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Models
{
    public class Farmer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Farmer Name")]
        [DataType(DataType.Text)]
        public string? FarmerName { get; set; }

        [Required]
        [Display(Name = "Farmer Surname")]
        [DataType(DataType.Text)]
        public string? FarmerSurname { get; set; }

        [Required]
        [Display(Name = "Farmer Age")]
        public int? Age { get; set; }

        [Required]
        [Display(Name = "Farmer Email")]
        [DataType(DataType.Text)]
        public string? Email { get; set; }

        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Text)]
        public string? Password { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.Text)]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Farmer Address")]
        [DataType(DataType.Text)]
        public string? Address { get; set; }
    }
}
