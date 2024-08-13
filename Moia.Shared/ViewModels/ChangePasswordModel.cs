namespace Moia.Shared.ViewModels
{
    public class ChangePasswordModel
    {
        public string Username { get; set; }
        public string? OldPassword { get; set; }

        [Required]
        [MaxLength(450)]
        public string NewPassword { get; set; }

    }



}
