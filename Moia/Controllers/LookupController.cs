using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moia.DoL.Enums;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;
using System.Linq;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : Controller
    {
        private readonly IUnitOfWork uow;
        public LookupController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        [HttpGet("GetAllDepartments")]
        public List<DepartmentDto> GetAllDepartments()
        {
            try
            {
                List<DepartmentDto> Depts = uow.DbContext.Department.AsNoTracking().Select(item =>
                new DepartmentDto
                {
                    ID = item.ID,
                    Title = item.Title,
                }).ToList();
                return Depts;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetAllBranchs")]
        public List<BranshDto> GetAllBranchs()
        {
            try
            {
                List<BranshDto> items = uow.DbContext.MinistryBranshs.AsNoTracking().Select(item =>
                new BranshDto
                {
                    ID = item.ID,
                    Title = item.Title,
                }).ToList();
                return items;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetAllDepartmentsPaginated")]
        [ProducesResponseType(typeof(ViewerPagination<DepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllDepartmentsPaginated(string? searchTerm, int page, int pageSize)
        {
            try
            {


                var myList = uow.Muslime.getAllDepartment(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAllBranchsPaginated")]
        [ProducesResponseType(typeof(ViewerPagination<BranshListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllBranchsPaginated(string? searchTerm, int page, int pageSize)
        {
            try
            {
                var myList = uow.Muslime.GetAllBranchs(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetResidencePalcePaginated")]
        [ProducesResponseType(typeof(ViewerPagination<ResidenceIssuePlace>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetResidencePalcePaginated(string? searchTerm, int page, int pageSize)
        {
            try
            {
                var myList = uow.Muslime.GetResidencePalcePaginated(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetResidencePalce")]
        public List<ResidenceIssuePlace> GetResidencePalce()
        {
            try
            {
                List<ResidenceIssuePlace> ResidenceIssuePlace = uow.DbContext.ResidenceIssuePlace.AsNoTracking().ToList();
                return ResidenceIssuePlace;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetPreshersPaginated")]
        [ProducesResponseType(typeof(ViewerPagination<Preacher>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetPreshersPaginated(string? searchTerm, int page, int pageSize)
        {
            try
            {
                var myList = uow.Muslime.GetPreshersPaginated(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetPreshers")]
        public List<Preacher> GetPreshers()
        {
            try
            {
                List<Preacher> Preachers = uow.DbContext.Preachers.AsNoTracking().ToList();
                return Preachers;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost("SavePresher")]
        public int SavePresher(Preacher model)
        {
            try
            {
                Preacher preacher = uow.DbContext.Preachers.FirstOrDefault(x => x.ID == model.ID);
                if (preacher == null) uow.DbContext.Preachers.Add(model);
                else
                {
                    preacher.Identity = model.Identity;
                    preacher.ContactNumber = model.ContactNumber;
                    preacher.Title = model.Title;
                }
                uow.SaveChanges();
                return model.ID;
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost("SaveResidenceIssuePlace")]
        public int SaveResidenceIssuePlace(ResidenceIssuePlace model)
        {
            try
            {
                ResidenceIssuePlace ResidenceIssuePlace = uow.DbContext.ResidenceIssuePlace.AsNoTracking().FirstOrDefault(x => x.ID == model.ID);
                if (ResidenceIssuePlace == null) uow.DbContext.ResidenceIssuePlace.Add(model);
                else
                {
                    ResidenceIssuePlace.Title = model.Title;
                }

                uow.SaveChanges();
                return model.ID;
            }
            catch
            {
                return 0;
            }
        }

        [HttpGet("RecognitionWays")]
        public List<IsslamRecognitionData> RecognitionWays()
        {
            try
            {
                List<IsslamRecognitionData> IslamRecognitionWay = uow.DbContext.IsslamRecognition.AsNoTracking().Select(x => new IsslamRecognitionData
                {
                    ID = x.ID,
                    Title = x.Title,
                }).ToList();
                return IslamRecognitionWay;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetCountries")]
        public List<Country> GetCountries()
        {
            try
            {
                List<Country> Countries = uow.DbContext.Countries.AsNoTracking().ToList();
                return Countries;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetEducationalLevels")]
        public List<EducationalLevel> GetEducationalLevels()
        {
            try
            {
                List<EducationalLevel> data = uow.DbContext.EducationalLevels.AsNoTracking().ToList();
                return data;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetReligions")]
        public List<Religion> GetReligions()
        {
            try
            {
                List<Religion> data = uow.DbContext.Religions.AsNoTracking().ToList();
                return data;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetMainRoles")]
        public List<MainRoleNameId> GetMainRoles()
        {
            try
            {
                List<MainRoleNameId> data = uow.DbContext.MainRoles.AsNoTracking().
                    Where(x => x.Code == UserType.BranchManager.ToString() ||
                             x.Code == UserType.DepartmentManager.ToString() ||
                             x.Code == UserType.BranchDataEntry.ToString()
                    ).
                    Select(x => new MainRoleNameId
                    {
                        ID = x.ID,
                        Name = x.Code,
                        NameAr = x.NameAr,
                    }).ToList();
                return data;
            }
            catch
            {
                return null;
            }
        }
        
        [HttpGet("GetMainRolesDataEntryCommitteeManger")]
        public List<MainRoleNameId> GetMainRolesDataEntryCommitteeManger()
        {
            try
            {
                List<MainRoleNameId> data = uow.DbContext.MainRoles.AsNoTracking().
                    Where(x => x.Code == RoleCodes.CommitteeManager.ToString() ||
                             x.Code == RoleCodes.DataEntry.ToString() 
                    ).
                    Select(x => new MainRoleNameId
                    {
                        ID = x.ID,
                        Name = x.Code,
                        NameAr = x.NameAr,
                    }).ToList();
                return data;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetAllMainRoles")]
        public List<MainRoleNameId> GetAllMainRoles()
        {
            try
            {
                List<MainRoleNameId> data = uow.DbContext.
                                                MainRoles.
                                                AsNoTracking().
                                                Where(x => x.NameAr != null).
                    Select(x => new MainRoleNameId
                    {
                        ID = x.ID,
                        Name = x.Code,
                        NameAr = x.NameAr,
                    }).ToList();
                return data;
            }
            catch
            {
                return null;
            }
        }


        [HttpGet("GetCommitteeMainRoles")]
        public List<MainRoleNameId> GetCommitteeMainRoles()
        {
            try
            {
                List<MainRoleNameId> data = uow.DbContext.MainRoles.AsNoTracking().
                    Where(x => x.Code == UserType.CommitteeManager.ToString() ||
                             x.Code == UserType.DataEntry.ToString()
                    ).
                    Select(x => new MainRoleNameId
                    {
                        ID = x.ID,
                        NameAr = x.NameAr,
                        Name = x.Code,
                    }).ToList();
                return data;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetAllUsers")]
        public List<CommitteeRoleViewModel> GetAllUsers()
        {
            try
            {
                List<CommitteeRoleViewModel> Depts = null;
                var branchId = uow.DbContext.MainUsers.AsNoTracking().FirstOrDefault(x => x.ID == uow.SessionServices.UserId)?.BranchId;
                if (branchId != null)
                {
                    var MainUserRole = uow.DbContext.MainUserRole.AsNoTracking().Select(x => x.UserId).ToList();
                    //.Where(x => !MainUserRole.Contains(x.ID))
                    Depts = uow.DbContext.MainUsers
                       .Where(x => x.BranchId == branchId)
                       .Select(item =>
                   new CommitteeRoleViewModel
                   {
                       ID = item.ID,
                       Title = item.Name,
                   }).ToList();
                }
                return Depts;
            }
            catch
            {
                return null;
            }
        }


        [HttpGet("GetRelatedUsers")]
        public List<NameIdViewModel> GetRelatedUsers(int EntryId)
        {
            try
            {
                var userId = uow.SessionServices.UserId;
                var userRole = uow.DbContext.MainUserRole
                                   .Include(x => x.Role)
                                   .FirstOrDefault(x => x.UserId == userId);

                var CommitteeId = uow.DbContext.
                                    Orders.Include(x=>x.DataEntry).
                                            Include(x=>x.Committee).
                                            FirstOrDefault(x => x.DataEntry.ID == EntryId)?.Committee.ID; 



                if (userRole == null) return new List<NameIdViewModel>();

                var userRoleCode = userRole.Role.Code;
                IQueryable<Committee> committeesQuery = uow.DbContext.Committees.Where(x => x.ID == CommitteeId);

                var committeesWithUsers = committeesQuery
                                            .Include(committee => committee.Users)
                                            .ThenInclude(userRole => userRole.User)
                                            .ToList();

                var users = committeesWithUsers
                            .SelectMany(committee => committee.Users)
                            .Where(userRole => userRole.User != null)
                            .Select(userRole => userRole.User)
                            .Where(x=>x.ID != EntryId)
                            .Select(user => new NameIdViewModel
                            {
                                ID = user.ID,
                                Title = user.Name
                            })
                            .ToList();

                return users;
            }
            catch
            {
                return null;
            }
        }


        [HttpGet("GetADUsers")]
        public List<CommitteeRoleViewModel> GetADUsers()
        {
            try
            {
                var MainUserRole = uow.DbContext.MainUserRole.AsNoTracking().Select(x => x.UserId).ToList();
                List<CommitteeRoleViewModel> Depts = uow.DbContext.MainUsers
                    .Where(x => !MainUserRole.Contains(x.ID) && x.ActiveDirectoryUser)
                    .Select(item =>
                new CommitteeRoleViewModel
                {
                    ID = item.ID,
                    Title = item.Name,
                }).ToList();
                return Depts;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetAllNegotiatedUsers")]
        public List<CommitteeRoleViewModel> GetAllNegotiatedUsers()
        {
            try
            {
                var branchId = uow.DbContext.MainUsers.AsNoTracking().FirstOrDefault(x => x.ID == uow.SessionServices.UserId)?.BranchId;
                if (branchId != null)
                {
                    var MainUserRole = uow.DbContext.MainUserRole.AsNoTracking().Select(x => x.UserId).ToList();

                    List<CommitteeRoleViewModel> res = uow.DbContext.MainUsers
                                                        .Where(x => !MainUserRole.Contains(x.ID) && x.ActiveDirectoryUser && x.BranchId == branchId)
                                                        .Select(item =>
                                                        new CommitteeRoleViewModel
                                                        {
                                                            ID = item.ID,
                                                            Title = item.Name,
                                                        }).ToList();
                    return res;
                }
            }
            catch
            {
            }
            return null;
        }



    }
}
