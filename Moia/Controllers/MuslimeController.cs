using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moia.DoL.Enums;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MuslimeController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public MuslimeController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public virtual async Task<List<MuslimeDto>> Index()
        {
            var Muslimes = await uow.Muslime.DbSet.ToListAsync();
            var MuslimesDtos = uow.Mapper.Map<List<MuslimeDto>>(Muslimes);
            return MuslimesDtos;
        }

        [HttpGet("getData")]
        public async Task<MuslimeDto> getData(string OrderCode)
        {
            var res = await uow.Muslime.getData(OrderCode);
            return res;
        }

        [AllowAnonymous]
        [HttpPost("getDataForQuery")]
        public async Task<UserDataForVieweing> getDataForQuery([FromQuery]string id)
        {
            var res = await uow.Muslime.getDataForQuery(id);
            return res;
        }
        
        [HttpGet("getPersonalData")]
        public async Task<PersonalDataDto> getPersonalData(int MuslimeId)
        {
            var res = await uow.Muslime.getPersonalData(MuslimeId);
            return res;
        }
        
        [HttpGet("getPersonalInformation")]
        public async Task<PersonalInformationDto> getPersonalInformation(int MuslimeId)
        {
            var res = await uow.Muslime.getPersonalInformation(MuslimeId);
            return res;
        }
        
        [HttpGet("getContactData")]
        public async Task<ContactAndInfoDataViewModel> getContactData(int MuslimeId)
        {
            var res = await uow.Muslime.getContactData(MuslimeId);
            return res;
        }
        
        [HttpGet("getFamilyAndWork")]
        public async Task<FamilyAndWorkDto> getFamilyAndWork(int MuslimeId)
        {
            var res = await uow.Muslime.getFamilyAndWork(MuslimeId);
            return res;
        }
        
        [HttpGet("getIslamRecognitionWay")]
        public async Task<List<IsslamRecognitionData>> getIslamRecognitionWay(int MuslimeId)
        {
            var res = await uow.Muslime.getIslamRecognitionWay(MuslimeId);
            return res;
        }
        
        [HttpGet("getattachmentDto")]
        public async Task<AttachmentDto> getattachmentDto(int MuslimeId)
        {
            var res = await uow.Muslime.getattachmentDto(MuslimeId);
            return res;
        }

       
        [HttpPost("CreatePersonalData")]
        public async Task<int> CreatePersonalData(PersonalDataDto model)
        {
          return await uow.Muslime.CreatePersonalData(model);
        }
        
        [HttpPost("CreatePersonalInformation")]
        public async Task<int> CreatePersonalInformation(PersonalInformationDto model)
        {
          return await uow.Muslime.CreatePersonalInformation(model);
        }
        
        
        [HttpPost("CreateContactAndInfoData")]
        public async Task<int> CreateContactAndInfoData(ContactAndInfoDataViewModel model)
        {
          return await uow.Muslime.CreateContactAndInfoData(model);
        }
        
        [HttpPost("CreateFamilyAndWorkDto")]
        public async Task<int> CreateFamilyAndWorkDto(FamilyAndWorkDto model)
        {
          return await uow.Muslime.CreateFamilyAndWorkDto(model);
        }
        
        [HttpPost("CreateIslamRecognitionWays")]
        public async Task<int> CreateIslamRecognitionWays(IslamRecognitionWayDto model)
        {
          return await uow.Muslime.CreateIslamRecognitionWays(model);
        }
        
        [HttpPost("InsertAttachment")]
        [ProducesResponseType(typeof(CustomeResponse), StatusCodes.Status200OK)]
        public async Task<CustomeResponse> InsertAttachment([FromForm] AttachmentDto model)
        {
          return await uow.Muslime.InsertAttachment(model);
        }
        
        [HttpPost("ChangeOrderState")]
        [ProducesResponseType(typeof(CustomeResponse), StatusCodes.Status200OK)]
        public async Task<CustomeResponse> ChangeOrderState(string OrderCode, OrderStatus orderStatus, OrderStage orderStage, string Description)
        {
          return await uow.Muslime.ChangeOrderState( OrderCode,  orderStatus,  orderStage,  Description);
        }


        [AllowAnonymous]
        [HttpGet("PrintCard")]
        public PrintCardView PrintCard(string Code)
        {
          return uow.Muslime.PrintCard(Code);
        } 
         
        [HttpPost("UpdateCardTawakkalna")]
        public async Task<bool> UpdateCardTawakkalna(string Code) 
        {
          return await uow.Muslime.UpdateTawakkalnaCard(Code);
        }
        
        [HttpPost("AddTawakkalnaCard")]
        public async Task<bool> AddTawakkalnaCard(string Code) 
        {
            return await uow.Muslime.AddTawakkalnaCard(Code);
        }
        
        [HttpPost("DeleteCardTawakkalna")]
        public async Task< bool> DeleteCardTawakkalna(string Code) 
        {
          return await uow.Muslime.DeleteTawakkalnaCard(Code);
        }
        
    }
} 
