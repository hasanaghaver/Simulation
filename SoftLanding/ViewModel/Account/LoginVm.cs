using System.ComponentModel.DataAnnotations;

namespace SoftLanding.ViewModel
{
    public class LoginVm
    {
        public string UsernameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
