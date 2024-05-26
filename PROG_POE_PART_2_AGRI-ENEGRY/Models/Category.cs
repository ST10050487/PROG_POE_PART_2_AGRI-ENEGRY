using System.ComponentModel.DataAnnotations;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        // Navigation property for the related products
        public ICollection<Product> Products { get; set; }
    }
}
