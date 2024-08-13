using Moia.DoL.Enums;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.BL.Repositories
{
    public interface ICommitteeRepository : IRepository<Committee>
    {
        ViewerPagination<CommitteeList> getWithPaginate(int page, int pageSize, string searchTerm);
        List<CommitteeIdName> getAllCommittee();
        List<CommitteeIdName> getAllCommitteesByBranchId(int branchId);

    }


    public class CommitteeRepository : Repository<Committee>, ICommitteeRepository
    {
        private readonly IUnitOfWork uow;

        public CommitteeRepository(IUnitOfWork _uow) : base(_uow)
        {
            uow = _uow;
        }


        public ViewerPagination<CommitteeList> getWithPaginate(int page, int pageSize, string searchTerm)
        {
            int? DepartmentId = null;
            var UserId = uow.SessionServices.UserId;
            var logedInUser = uow.DbContext.MainUsers.Include(x => x.MainUserRole).ThenInclude(x => x.Role).AsNoTracking().FirstOrDefault(x => x.ID == UserId);
            if (logedInUser != null)
                DepartmentId = uow.DbContext.Department.AsNoTracking().Where(x => x.BranshID == logedInUser.BranchId).FirstOrDefault()?.ID;

            IQueryable<Committee> myData;
            int myDataCount = 0;
            //if (!string.IsNullOrEmpty(searchTerm))
            //{
            if (logedInUser.MainUserRole.FirstOrDefault().Role.Code == nameof(UserType.SuperAdmin))
            {
                myData = uow.DbContext.Committees.Include(x => x.Users).
                                                              ThenInclude(x => x.Role)
                                           .Include(x => x.Users).
                                                              ThenInclude(x => x.User)
                                            .Include(x => x.Department).
                                                              ThenInclude(x => x.MinistryBransh)
                                              .Where(a => searchTerm == null ||
                                                 a.Title.Contains(searchTerm) ||
                                                 a.Code.Contains(searchTerm) ||
                                                 a.ContactNumber.Contains(searchTerm) ||
                                                  a.Users.Any(u => u.User.Name.Contains(searchTerm)) ||
                                                 a.Users.Any(u => u.User.UserName.Contains(searchTerm)) ||
                                                 a.Department.Title.Contains(searchTerm) ||
                                                 a.Department.MinistryBransh.Title.Contains(searchTerm));
            }
            else
            {
                myData = uow.DbContext.Committees
                                     .Include(x => x.Users).ThenInclude(x => x.Role)
                                     .Include(x => x.Users).ThenInclude(x => x.User)
                                     .Include(x => x.Department).ThenInclude(x => x.MinistryBransh)
                                     .Where(a => searchTerm == null ||
                                                 a.Title.Contains(searchTerm) ||
                                                 a.ContactNumber.Contains(searchTerm) ||
                                                 a.Code.Contains(searchTerm) ||
                                                  a.Users.Any(u => u.User.Name.Contains(searchTerm)) || 
                                                  a.Users.Any(u => u.User.UserName.Contains(searchTerm)) ||
                                                 a.Department.Title.Contains(searchTerm) ||
                                                 a.Department.MinistryBransh.Title.Contains(searchTerm))
                                     .Where(x => x.DepartmentId == DepartmentId);

            }
            //}
            //else
            //{
            //    if (logedInUser.MainUserRole.FirstOrDefault().Role.Name == nameof(UserType.SuperAdmin))
            //    {
            //        myData = uow.DbContext.Committees.Include(x => x.Users).
            //                                                      ThenInclude(x => x.Role)
            //                                   .Include(x => x.Users).
            //                                                      ThenInclude(x => x.User)
            //                                    .Include(x => x.Department).
            //                                                      ThenInclude(x => x.MinistryBransh);
            //    }
            //    else
            //    {
            //        myData = uow.DbContext.Committees.Include(x => x.Users).
            //                                                            ThenInclude(x => x.Role)
            //                                         .Include(x => x.Users).
            //                                                            ThenInclude(x => x.User)
            //                                          .Include(x => x.Department).
            //                                                            ThenInclude(x => x.MinistryBransh)
            //                                           .Where(x => x.DepartmentId == DepartmentId);
            //    }
            //}
            myDataCount = myData.Count();
            ViewerPagination<CommitteeList> viewerPagination = new ViewerPagination<CommitteeList>();

            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new CommitteeList
            {
                ID = x.ID,
                Title = x.Title,
                ContactNumber = x.ContactNumber,
                Code = x.Code,
                DepartmentDto = x.Department != null ? new DepartmentDto { ID = x.Department.ID, Title = x.Department.Title } : null,
                CommitteeManager = x.CommitteeManager,
                CommitteeDataEntryUsers = x.CommitteeDataEntryUsers,
                branchId = x.Department.BranshID,
                BranchTitle = x.Department.MinistryBransh.Title
            }).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }
        public List<CommitteeIdName> getAllCommittee()
        {
            var res = uow.Committee.DbSet.Select(x => new CommitteeIdName
            {
                ID = x.ID,
                Title = x.Title,
            }).ToList();
            return res;
        }

        public List<CommitteeIdName> getAllCommitteesByBranchId(int branchId)
        {
            var res = uow.Committee.DbSet.Include(x => x.Department).
                                          ThenInclude(x => x.MinistryBransh).
                                          Where(x => x.Department.MinistryBransh.ID == branchId).
                                          Select(x => new CommitteeIdName
                                          {
                                              ID = x.ID,
                                              Title = x.Title,
                                          }).ToList();
            return res;
        }

    }


}
