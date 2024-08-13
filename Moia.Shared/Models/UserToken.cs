namespace Moia.Shared.Models
{
    public class UserToken : _Model
    {
        public string AccessTokenHash { get; set; }

        public DateTimeOffset AccessTokenExpiresDateTime { get; set; }

        [MaxLength(450)]
        public string RefreshTokenIdHash { get; set; }

        [MaxLength(450)]
        public string RefreshTokenIdHashSource { get; set; }

        public DateTimeOffset RefreshTokenExpiresDateTime { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        // Apllication Type Enum ( ApplicationTypeEnum )
        public int? ApplicationType { get; set; }
        public virtual MainUser User { get; set; }
    }

}