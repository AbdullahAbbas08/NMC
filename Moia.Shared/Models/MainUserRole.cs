
namespace Moia.Shared.Models
{
    public class MainUserRole : _Model
    {
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public int? CommitteeId { get; set; }
        public int? DepartmentId  { get; set; }
        public int? BranshId  { get; set; }

        [NotMapped]
        public string RoleTitle { get => Role !=null ? Role.Code.ToString():null; }


        [ForeignKey("CommitteeId")]
        public virtual Committee Committee { get; set; }
        
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; } 
        
        [ForeignKey("BranshId")] 
        public virtual MinistryBransh Bransh { get; set; } 

        [ForeignKey("UserId")]
        public virtual MainUser User  { get; set; }

        [ForeignKey("RoleId")]
        public virtual MainRole Role { get; set; }
    }
}
