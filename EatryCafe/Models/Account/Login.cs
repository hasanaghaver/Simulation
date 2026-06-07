using System.ComponentModel.DataAnnotations;

namespace EatryCafe.Models
{
    public class Login
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string UsernameOrEmail { get; set; }
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
