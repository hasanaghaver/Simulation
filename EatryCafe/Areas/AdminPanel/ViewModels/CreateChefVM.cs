using System.ComponentModel.DataAnnotations;

namespace EatryCafe.Areas.AdminPanel.ViewModels
{
    public class CreateChefVM
    {
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Job { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
