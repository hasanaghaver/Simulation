using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace EatryCafe.Models
{
    public class Register
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
