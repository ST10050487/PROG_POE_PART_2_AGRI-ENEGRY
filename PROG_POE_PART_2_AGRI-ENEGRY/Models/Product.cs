using PROG_POE_PART_2_AGRI_ENEGRY.Areas.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        [DataType(DataType.Text)]
        public string? Name { get; set; }

        [Display(Name = "Product Picture")]
        public string? PictureUrl { get; set; }

        [Display(Name = "Product Price")]
        public int Price { get; set; }

        [Required]
        [Display(Name = "Production Date")]
        [DataType(DataType.Date)]
        public DateTime ProductionDate { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public PlatformUsers User { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
