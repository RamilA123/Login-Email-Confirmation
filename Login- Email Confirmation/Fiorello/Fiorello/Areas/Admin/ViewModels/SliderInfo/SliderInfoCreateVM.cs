using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.Admin.ViewModels.SliderInfo
{
    public class SliderInfoCreateVM
    {
        [Required]
        public IFormFile Images { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Info { get; set; }
    }
}
