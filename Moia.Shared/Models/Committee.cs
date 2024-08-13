using Moia.Shared.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace Moia.Shared.Models
{
    public class Committee : _Model
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public string ContactNumber { get; set; }
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<MainUserRole> Users { get; set; }

        [NotMapped]
        public CommitteeRoleViewModel CommitteeManager
        {
            get
            {

                var RoleCommitteeManager = Users?.FirstOrDefault(x => x.CommitteeId == ID &&
                                                  x.RoleTitle == UserType.CommitteeManager.ToString());
                if (RoleCommitteeManager == null || RoleCommitteeManager.User == null) return null;
                var res = new CommitteeRoleViewModel
                {
                    ID = RoleCommitteeManager.User?.ID,
                    Title = RoleCommitteeManager.User?.Name
                };
                return res;


            }
        }

        [NotMapped]
        public List<CommitteeRoleViewModel> CommitteeDataEntryUsers
        {
            get
            {

                var CommitteeDataEntry = Users?
                                          .Where(x => x.CommitteeId == ID && x.RoleTitle == UserType.DataEntry.ToString())
                                          .Select(item =>
                                          {
                                              var user = item.User;
                                              if (user != null)
                                              {
                                                  return new CommitteeRoleViewModel
                                                  {
                                                      ID = user.ID,
                                                      Title = user.Name
                                                  };
                                              }
                                              return null;
                                          })
                                          .ToList();
                var t =  CommitteeDataEntry.Where(x=> x !=null).ToList();
                return t;
            }
        }
    }
}
