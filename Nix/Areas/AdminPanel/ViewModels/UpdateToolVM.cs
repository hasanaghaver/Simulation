using System.ComponentModel.DataAnnotations;

namespace Nix.Areas.AdminPanel.ViewModels
{
    public class UpdateToolVM
    {
        public IFormFile? Photo { get; set; }

        public string Image { get; set; }
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
