
using Moia.Shared.ViewModels;

namespace Moia.Shared.Models
{
    public class MainUser :_Model
    {

        public UserType UserType  { get; set; }
        public string Name { get; set; }
        public string Identity { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<MainUserRole> MainUserRole  { get; set; }
        public int TryloginCount { get; set; }
        public bool ActiveDirectoryUser { get; set; }
        public bool Display { get; set; } = true;
        public bool PasswordChanged { get; set; }
        public int?  BranchId { get; set; }

        [ForeignKey("BranchId")]
        public virtual MinistryBransh Bransh { get; set; }
        public DateTime? OTPCreationTime { get; set; }
        public string OTP { get; set; }
        public bool Active  { get; set; }
    }

    public class MainUserDto 
    {
        public int ID { get; set; }
        public UserType UserType  { get; set; }
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public virtual Committee Committee { get; set; }
        public virtual Department Department { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public string UserName { get; set; }
        public string RoleTitle { get; set; }
        public bool Enable { get; set; }
    }
    
    public class MainUserNameIDList
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class UserDto
    {
        public int ID { get; set; }
        [MaxLength(450)]
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Email { get; set; }
        public bool ActiveDirectoryUser { get; set; }
        public BranshDto? Branch { get; set; }
        public int? BranchId { get; set; }
        public UserType? UserType { get; set; }
        public MainRoleNameId Role { get; set; }
        public int? RoleId { get; set; }
        //public int? UserTypeId { get; set; }
        public IFormFile Signature  { get; set; }
        public int? CommitteeId { get; set; }
        public string AttachmentBase64 { get; set; }
    }

}
