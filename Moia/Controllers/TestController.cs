using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moia.DoL.Enums;
using Moia.Shared.Encryption;
using Moia.Shared.Models;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        } 

        [AllowAnonymous]
        [HttpGet("EncryptValue")]
        public string EncryptConnectionString(string ConnectionString)
        {
            return EncryptHelper.Encrypt(ConnectionString);
        }


        [AllowAnonymous]
        [HttpGet("DecryptValue")]
        public string DecryptConnectionString(string ConnectionString)
        {
            return EncryptHelper.Decrypt(ConnectionString);
        }
        
        [AllowAnonymous]
        [HttpGet("DecryptResponse")]
        public string DecryptResponse(string ConnectionString)
        {
            return Encription.DecryptStringAES(ConnectionString);
        }

    }
}