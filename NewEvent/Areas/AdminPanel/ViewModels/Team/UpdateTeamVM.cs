using NewEvent.Models;
using System.ComponentModel.DataAnnotations;

namespace NewEvent.Areas.AdminPanel.ViewModels
{
    public class UpdateTeamVM
    {
        public IFormFile? Photo { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int JobID { get; set; }
        public string? Image { get; set; }
        public List<Job>? Jobs { get; set; }
    }
}
