using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Nix.Areas.AdminPanel.ViewModels
{
    public class CreateToolVM
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(80)]
        public string SubTitle { get; set; }
    }
}
