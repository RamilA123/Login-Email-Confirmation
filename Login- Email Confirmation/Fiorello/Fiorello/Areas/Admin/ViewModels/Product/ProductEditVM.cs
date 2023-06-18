using System.ComponentModel.DataAnnotations;
using Fiorello.Models;

namespace Fiorello.Areas.Admin.ViewModels.Product
{
    public class ProductEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public int DiscountId { get; set; }
        public ICollection<Image> Images { get; set; }
        public List<IFormFile> NewImage { get; set; }
    }
}
