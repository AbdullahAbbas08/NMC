using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Shared.ViewModels
{
    public class CommitteeDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int? branchId { get; set; }
        public string ContactNumber { get; set; }
        public DepartmentDto? DepartmentDto { get; set; }
        public CommitteeRoleViewModel CommitteeManager { get; set; }
        public List<CommitteeRoleViewModel> committeeDataEntryUsers { get; set; }
    }

  
}
