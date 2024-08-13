using Moia.Shared.ViewModels;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Shared.Models
{
    public class MinistryBransh : _Model
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int? UserRoleID { get; set; }
        [ForeignKey("UserRoleID")]
        public virtual MainUserRole UserRole { get; set; }
        public virtual Department Department { get; set; }

        public virtual ICollection<MainUser> Users  { get; set; }

    }

     
    public class MinistryBranshDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string MangerRoleID { get; set; }
        public DepartmentDto Department { get; set; }
    }
    
    public class BranshListDto 
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public NameIdViewModel Manager { get; set; }
    }
    
    public class BranshDto  
    {
        public int? ID { get; set; }
        public string Title { get; set; }
    }
}
