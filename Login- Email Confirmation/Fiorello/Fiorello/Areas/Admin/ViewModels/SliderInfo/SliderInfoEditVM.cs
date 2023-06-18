using System.ComponentModel.DataAnnotations;

namespace Fiorello.Areas.Admin.ViewModels.SliderInfo
{
    public class SliderInfoEditVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Info { get; set; }
        public string Image { get; set; }
        [Required]
        public IFormFile NewImage { get; set; }
    }
}
