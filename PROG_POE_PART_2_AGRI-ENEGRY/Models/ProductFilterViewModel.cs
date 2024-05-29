using Microsoft.AspNetCore.Mvc.Rendering;

namespace PROG_POE_PART_2_AGRI_ENEGRY.Models
{
    public class ProductFilterViewModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CategoryId { get; set; }
        public string FarmerId { get; set; }
        public List<Product> Products { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Farmers { get; set; }
    }
}
