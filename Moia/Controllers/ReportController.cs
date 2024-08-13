using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moia.Shared.Models;
using System.Collections.Generic;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        public ReportController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        [HttpGet("getStatisticalsOrders")]
        [ProducesResponseType(typeof(ViewerPagination<StatisticalsOrders>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetStatisticalsOrders(int? branchID, int? committeeId, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            ViewerPagination<StatisticalsOrders> statisticalsOrders = await uow.Report.GetStatisticalOrders(branchID, committeeId ,fromDate, toDate, page, pageSize);
            return Ok(statisticalsOrders);
        }
    }
}
