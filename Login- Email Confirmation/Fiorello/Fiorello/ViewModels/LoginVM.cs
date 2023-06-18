using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string EmailOrUsername { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
