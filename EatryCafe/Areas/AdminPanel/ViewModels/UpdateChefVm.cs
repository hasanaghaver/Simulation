using System.ComponentModel.DataAnnotations;

namespace EatryCafe.Areas.AdminPanel.ViewModels
{
    public class UpdateChefVm
    {
        public IFormFile? Photo { get; set; }
        public string Image { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Job { get; set; }
        [Required]
        [MinLength(3)]
        public string Description { get; set; }
    }
}
