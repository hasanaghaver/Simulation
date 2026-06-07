using System.ComponentModel.DataAnnotations;

namespace Nix.ViewModels
{
    public class LoginVm
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string UsernameOrEmail { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}
