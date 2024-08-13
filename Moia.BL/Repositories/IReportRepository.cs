using Microsoft.Reporting.Map.WebForms.BingMaps;
using Moia.DoL.Enums;
using Moia.Shared.Models;
using Moia.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Moia.BL.Repositories
{
    public interface IReportRepository

    {
        Task<ViewerPagination<StatisticalsOrders>> GetStatisticalOrders(int? branchID, int? committeeId, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
    }
    public class ReportRepository : IReportRepository
    {
        private readonly IUnitOfWork uow;
        public ReportRepository(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public async Task<ViewerPagination<StatisticalsOrders>> GetStatisticalOrders(int? branchID, int? committeeId, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            try
            {
                var UserId = uow.SessionServices.UserId;
                var UserRole = await uow.DbContext.MainUserRole.Include(u=> u.Role).FirstOrDefaultAsync(u => u.UserId == UserId);
                
                List<StatisticalsOrders> myData = new List<StatisticalsOrders>();                
                if (UserRole != null)
                {
                    List<Committee> committees = new List<Committee>();

                    switch (UserRole.Role.Code)
                    {
                        case RoleCodes.CommitteeManager:
                            committees = await uow.Committee.DbSet.Include(c => c.Orders)
                                                    .Where(c => c.ID == UserRole.CommitteeId)
                                                    .Where(c => c.Orders.Count > 0).ToListAsync();
                            break;

                        case RoleCodes.DepartmentManager:
                            committees = await uow.Committee.DbSet
                                                    .Include(c => c.Orders)
                                                    .Where(x => x.DepartmentId == UserRole.DepartmentId)
                                                    .Where(x => committeeId == null || x.ID == committeeId)
                                                    .Where(c => c.Orders.Count > 0).ToListAsync();
                            break;

                        case RoleCodes.BranchManager:
                            committees = await uow.Committee.DbSet
                                        .Include(c => c.Orders)
                                        .Include(c => c.Department)
                                        .ThenInclude(d => d.MinistryBransh)
                                        .Where(c => c.Department.BranshID == UserRole.BranshId)
                                        .Where(x => committeeId == null || x.ID == committeeId)
                                        .Where(c => c.Orders.Count > 0).ToListAsync();
                            break;
                        case RoleCodes.SuperAdmin:
                            committees = await uow.Committee.DbSet.Include(c => c.Orders)
                                                            .Include(c => c.Department)
                                                            .Where(c => branchID == null || c.Department.BranshID == branchID)
                                                            .Where(x => committeeId == null || x.ID == committeeId)
                                                            .Where(c => c.Orders.Count > 0).ToListAsync();
                            break;

                        default:
                            break;
                    }

                    foreach (var committee in committees)
                    {
                        myData.Add(new StatisticalsOrders
                        {
                            CommitteeTitle = committee.Title,
                            OrdersNum = committee.Orders
                                         .Where(o => (fromDate == null || o.CreationDate >= fromDate)
                                                        && (toDate == null || o.CreationDate <= toDate)).
                                                        Count(),
                            ConfirmedOrdersNum = committee.Orders
                                                .Where(o => o.Stage == OrderStage.ReadyToPrintCard
                                                        && (fromDate == null || o.CreationDate >= fromDate)
                                                        && (toDate == null || o.CreationDate <= toDate))
                                                .Count(),
                            NotConfirmedOrdersNum = committee.Orders
                                                    .Where(o => (o.Stage == OrderStage.Committee
                                                            || o.Stage == OrderStage.Branch
                                                            || o.Stage == OrderStage.Department
                                                            || o.Stage == OrderStage.DataEntry
                                                            || o.Stage == OrderStage.Other)
                                                            && (fromDate == null || o.CreationDate >= fromDate)
                                                        && (toDate == null || o.CreationDate <= toDate))
                                                    .Count()
                        });
                    }

                    myData = myData.Where(s => s.OrdersNum > 0).ToList();

                    int myDataCount = myData.Count();

                    ViewerPagination<StatisticalsOrders> viewerPagination = new ViewerPagination<StatisticalsOrders>
                    {
                        PaginationList = myData.Skip((page - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToList(),
                        OriginalListListCount = myDataCount
                    };

                    return viewerPagination;
                }
                else
                {
                    uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                    {
                        Message = "Error Occured in ReportRepository",
                        Stacktrace = myData.ToString()
                    });
                    uow.SaveChanges();
                    return null;
                }
            }
            catch (Exception ex) {
                uow.DbContext.Exceptions.Add(new Shared.Models.Exceptions
                {
                    Message = "Error Occured in ReportRepository",
                    Stacktrace = ex.Message + "  StackTrace : " + ex.StackTrace,
                });
                uow.SaveChanges();
                return null;
            }
        }
    }
}