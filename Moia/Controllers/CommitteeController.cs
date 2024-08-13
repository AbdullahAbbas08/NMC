using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moia.DoL.Enums;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteeController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        public CommitteeController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        [HttpGet("GetById")]
        public virtual CommitteeList Get(int id)
        {
            try
            {
                CommitteeList _CommitteeList = new();
                var res = uow.Committee.DbSet.
                    Include(x => x.Users).
                    ThenInclude(x => x.Role).
                    Include(x => x.Users).
                    ThenInclude(x => x.User).
                    Include(x => x.Department).
                    FirstOrDefault(x => x.ID == id);
                if (res != null)
                {
                    _CommitteeList.ID = res.ID;
                    _CommitteeList.Title = res.Title;
                    _CommitteeList.DepartmentDto = res.Department != null ? new DepartmentDto() { ID = res.Department.ID, Title = res.Department.Title } : null;
                    _CommitteeList.CommitteeManager = res.CommitteeManager;
                    _CommitteeList.CommitteeDataEntryUsers = res.CommitteeDataEntryUsers;
                }
                if (res == null) return null;
                return _CommitteeList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetCommiteeDataEntries")]
        public virtual List<MainUserNameIDList> GetCommiteeDataEntries(int id)
        {
            try
            {
                var Committee = uow.Committee.DbSet.
                    Include(x => x.Users).
                    ThenInclude(x => x.Role).
                    Include(x => x.Users).
                    ThenInclude(x => x.User).
                    Include(x => x.Department).
                    FirstOrDefault(x => x.ID == id);

                var users = Committee?.Users?.Where(x => x.RoleTitle == UserType.DataEntry.ToString())?.
                    Select(x => new MainUserNameIDList
                    {
                        ID = x.ID,
                        Name = x.User?.Name,
                    }).ToList();

                if (users == null) return null;
                return users;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("getAllCommittees")]
        public virtual List<CommitteeIdName> getAllCommittees()
        {
            try
            {
                return uow.Committee.getAllCommittee();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("getAllCommitteesByBranchId")]
        public virtual List<CommitteeIdName> getAllCommitteesByBranchId(int branchId)
        {
            try
            {
                return uow.Committee.getAllCommitteesByBranchId(branchId);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("GetAllCustom")]
        [ProducesResponseType(typeof(ViewerPagination<CommitteeList>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAll(string? searchTerm, int page, int pageSize)
        {
            try
            {
                var myList = uow.Committee.getWithPaginate(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("getCommitteeOrders")]
        [ProducesResponseType(typeof(ViewerPagination<OrderListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult getCommitteeOrders(string? searchTerm, int page, int pageSize, int? committeeId = null, int? departmentId = null, List<OrderStatus?> orderStatus = null)
        {
            try
            {
                var myList = uow.Muslime.getOrders(page, pageSize, searchTerm, committeeId, departmentId, orderStatus);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("getOrdersForPreview")]
        [ProducesResponseType(typeof(ViewerPagination<OrderListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult getOrdersForPreview(string? searchTerm, int page, int pageSize, int? committeeId = null, int? departmentId = null, List<OrderStatus?> orderStatus = null)
        {
            try
            {
                var myList = uow.Muslime.getOrdersForPreview(page, pageSize, searchTerm, committeeId, departmentId, orderStatus);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("getFinishedOrders")]
        [ProducesResponseType(typeof(ViewerPagination<OrderListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult getFinishedOrders(string? searchTerm, int page, int pageSize, int? committeeId = null, int? departmentId = null)
        {
            try
            {
                var myList = uow.Muslime.getFinishedOrders(page, pageSize, searchTerm, committeeId, departmentId);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Insert")]
        public async Task<GenericResult<Committee>> Insert(CommitteeDto model)
        {
            try
            {
                Committee res;
                if (model.ID == 0)
                {
                    if (model.branchId == null)
                    {
                        var UserId = uow.SessionServices.UserId;
                        model.branchId = uow.DbContext.MainUsers.AsNoTracking().FirstOrDefault(x => x.ID == UserId)?.BranchId;

                    }
                    var commi = uow.DbContext.Committees.Include(x => x.Department).ThenInclude(x => x.MinistryBransh).AsNoTracking().Where(x => x.Department.BranshID == model.branchId);
                    var CommitteesCount = commi != null ? commi.Count() + 1 : 0;

                    // add Committee 
                    res = uow.Committee.Add(new Committee
                    {
                        Title = model.Title,
                        Code = CommitteesCount.ToString(),
                        ContactNumber = model.ContactNumber,
                        Department = uow.Mapper.Map<Department>(uow.DbContext.Department.Include(x => x.MinistryBransh).FirstOrDefault(x => x.MinistryBransh.ID == model.branchId)),
                    });

                    if (res == null)
                        return new GenericResult<Committee>()
                        {
                            Data = null,
                            Message = null,
                            Status = StatusCodes.Status400BadRequest
                        };
                    uow.SaveChanges();


                    // add committee Manager
                    //if (model.CommitteeManager != null)
                    //{
                    //    var manager = new MainUserRole
                    //    {
                    //        CommitteeId = res.ID,
                    //        RoleId = uow.DbContext.MainRoles.FirstOrDefault(x => x.Name == UserType.CommitteeManager.ToString()).ID,
                    //        UserId = model.CommitteeManager.ID
                    //    };
                    //    if (res.Users == null) { res.Users = new List<MainUserRole>(); }
                    //    res.Users.Add(manager);

                    //}

                    // add Data Entry 
                    //if (model.committeeDataEntryUsers != null)
                    //{
                    //    if (res.Users == null) { res.Users = new List<MainUserRole>(); }
                    //    foreach (var item in model.committeeDataEntryUsers)
                    //    {
                    //        var user = new MainUserRole
                    //        {
                    //            CommitteeId = res.ID,
                    //            RoleId = uow.DbContext.MainRoles.FirstOrDefault(x => x.Name == UserType.DataEntry.ToString()).ID,
                    //            UserId = item.ID
                    //        };
                    //        res.Users.Add(user);
                    //    }
                    //}
                    //uow.SaveChanges();
                }
                else
                {
                    res = await uow.Committee.DbSet.Include(x => x.Users).FirstOrDefaultAsync(x => x.ID == model.ID);
                    if (res != null)
                    {
                        res.Title = model.Title;
                        var branch = uow.DbContext.MinistryBranshs.Include(x => x.Department).FirstOrDefault(x => x.ID == model.branchId);
                        res.ContactNumber = model.ContactNumber;
                        res.Department = branch.Department;

                        //var query = uow.DbContext.MainUserRole.Include(x => x.Role);

                        //var manager = query.FirstOrDefault(x => x.CommitteeId == model.ID &&
                        //                      x.Role.Name == UserType.CommitteeManager.ToString());
                        //if (manager != null)
                        //{
                        //    manager.UserId = model.CommitteeManager.ID;
                        //}
                        //else
                        //{
                        //    var _manager = new MainUserRole
                        //    {
                        //        CommitteeId = res.ID,
                        //        RoleId = uow.DbContext.MainRoles.FirstOrDefault(x => x.Name == UserType.CommitteeManager.ToString()).ID,
                        //        UserId = model.CommitteeManager.ID
                        //    };
                        //    if (res.Users == null) { res.Users = new List<MainUserRole>(); }
                        //    res.Users.Add(_manager);
                        //}

                        //var CommitteeDataEntryRole = query.Where(x => x.CommitteeId == model.ID &&
                        //                      x.Role.Name == UserType.DataEntry.ToString()).ToList();

                        //uow.DbContext.MainUserRole.RemoveRange(CommitteeDataEntryRole);
                        //uow.SaveChanges();

                        //int RoleID = uow.DbContext.MainRoles.FirstOrDefault(x => x.Name == UserType.DataEntry.ToString()).ID;

                        //foreach (var item in model.committeeDataEntryUsers)
                        //{
                        //    uow.DbContext.MainUserRole.Add(new MainUserRole
                        //    {
                        //        UserId = item.ID,
                        //        RoleId = RoleID,
                        //        CommitteeId = res.ID
                        //    });
                        //}

                        uow.SaveChanges();
                    }
                    else
                    {
                        return new GenericResult<Committee>()
                        {
                            Data = null,
                            Message = null,
                            Status = StatusCodes.Status400BadRequest
                        };
                    }
                }
                return new GenericResult<Committee>()
                {
                    Data = res,
                    Message = null,
                    Status = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new GenericResult<Committee>()
                {
                    Data = null,
                    Message = ex.Message,
                    Status = StatusCodes.Status400BadRequest
                };
            }
        }

        [HttpPost("Delete")]
        public virtual async Task<bool> Delete(int id)
        {
            IEnumerable<object> ids = new List<object>() { id };
            try
            {
                var res = await uow.Committee.FindAsync(id);
                if (res != null)
                {
                    uow.Committee.Remove(res);
                    uow.SaveChanges();
                }
                else return false;
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        [HttpPost("TransfereOrders")]
        public async Task<OrderTransfereDTO> TransfereOrders(OrderTransfereDTO OrderTransfere)
        {
            try
            {

                var Orders = uow.DbContext.Orders.Include(x => x.DataEntry).Where(x => x.DataEntry.ID == OrderTransfere.FromUser);

                if (Orders.Any())
                {
                    await uow.DbContext.OrderTransfere.AddAsync(new OrderTransfere
                    {
                        FromUserId = OrderTransfere.FromUser,
                        ToUserId = OrderTransfere.ToUser,
                        Orders = Orders.ToList(),
                    });

                    MainUser user = await uow.DbContext.MainUsers.FirstOrDefaultAsync(x => x.ID == OrderTransfere.ToUser);
                    if (user != null)
                    {
                        foreach (var order in Orders)
                        {
                            order.DataEntry = user;
                        }

                        uow.DbContext.SaveChanges();
                    }
                }

                return OrderTransfere;
            }
            catch (Exception ex)
            {
                return null;    
            }
        }



    }
}
