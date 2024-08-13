using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moia.Shared.Models;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        public SettingsController(IUnitOfWork _uow)
        {
            uow = _uow; 
        }

        [AllowAnonymous]
        [HttpGet("GetSettings")]
        public virtual List<Settings> GetAll()
        {
            try
            {
                var res = uow.DbContext.Settings.ToList();
                return res;
            }
            catch
            {
                return null;
            }
        }
    }
}
