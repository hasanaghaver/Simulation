using System.ComponentModel.DataAnnotations;

namespace NewEvent.Areas.AdminPanel.ViewModels
{
    public class UpdateJobVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
