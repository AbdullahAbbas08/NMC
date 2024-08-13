
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public LocalizationController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("json/{culture}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public async Task<IActionResult> Json(string culture)
        {
            try
            {
                var myList = unitOfWork.LocalizationRepository.GetAllWithoutTracking();
                return Ok(unitOfWork.LocalizationRepository.GetJson(myList));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("getLocalization")]
        [ProducesResponseType(typeof(ViewerPagination<LocalizationDetailsDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> getLocalization(string? searchTerm, int page, int pageSize)
        {
            try
            {
                var myList = unitOfWork.LocalizationRepository.getTranslations(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("getLocalizationById")]
        [ProducesResponseType(typeof(LocalizationDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> getLocalizationById(int Id)
        {
            try
            {
                var Translation = unitOfWork.LocalizationRepository.getTranslationsById(Id);
                return Ok(Translation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("updateTranslation")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> updateTranslation([FromQuery] int Id, [FromBody] LocalizationDetailsDTO localization)
        {
            try
            {
                //int userId = unitOfWork.SessionServices.UserId.Value;
                int userId = 0;

                localization.Id = Id;
                var status = unitOfWork.LocalizationRepository.updatTranslation(localization, userId);
                unitOfWork.SaveChanges();
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [Route("deleteTranslation")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> deleteTranslation(int id)
        {
            try
            {
                var status = unitOfWork.LocalizationRepository.deleteTranslation(id);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("addTranslation")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> addTranslation([FromBody] LocalizationDetailsDTO localization)
        {
            try
            {
                var status = unitOfWork.LocalizationRepository.addTranslation(localization);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Route("addBulkTranslation")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<IActionResult> addBulkTranslation([FromBody] List<LocalizationDetailsDTO> localizations)
        {
            try
            {
                int userId = unitOfWork.SessionServices.UserId.Value;
                var status = unitOfWork.LocalizationRepository.addBulkTranslation(localizations, userId);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetLastUpDateTime")]
        [ProducesResponseType(typeof(DateTime), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLastUpDateTime()
        {
            try
            {
                //var myList = unitOfWork.LocalizationRepository.GetAllWithoutTracking();
                //return Ok(unitOfWork.LocalizationRepository.GetLastLocalizationUpdateTime(myList));
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
