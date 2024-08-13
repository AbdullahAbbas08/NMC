using Moia.Shared.Models;

namespace Moia.Shared.ViewModels
{
    public class UserLoginModel
    {
        [Required]
        [MaxLength(450)]
        public string Username { get; set; }
        [Required]
        [MaxLength(450)]
        public string Password { get; set; }

        public string culture { get; set; }
        public bool Continue { get; set; } = false;
    }

}
