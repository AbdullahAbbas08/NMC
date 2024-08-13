using Moia.Shared.ViewModels.DTOs;

namespace Moia.Shared.ViewModels
{
    public class CommitteeList
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int? branchId { get; set; }
        public string BranchTitle { get; set; }
        public string ContactNumber { get; set; }
        public DepartmentDto DepartmentDto  { get; set; } 
        public List<CommitteeRoleViewModel> CommitteeDataEntryUsers { get; set; }
        public CommitteeRoleViewModel CommitteeManager { get; set; }
    }


}
 