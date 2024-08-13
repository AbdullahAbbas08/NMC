using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IslamRecognitionWayController : ControllerBase
    {
        private readonly IUnitOfWork uow;

        public IslamRecognitionWayController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        [HttpGet("GetById")]
        public virtual IsslamRecognition Get(int id)
        {
            try
            {
                var res = uow.IslamRecognition.Get().FirstOrDefault(x => x.ID == id);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpGet]
        [Route("GetAllCustom")]
        [ProducesResponseType(typeof(ViewerPagination<IsslamRecognitionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAll(string? searchTerm, int page, int pageSize) 
        {
            try
            {
                var myList = uow.IslamRecognition.getWithPaginate(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RecognitionWay")]
        public async Task<GenericResult<IsslamRecognition>> InsertRecognitionWay(IsslamRecognition model)
        {
            try
            {
                IsslamRecognition res;
                if (model.ID == 0)
                {
                    res = uow.IslamRecognition.Add(model);

                    if (res == null)
                        return new GenericResult<IsslamRecognition>()
                        {
                            Data = null,
                            Message = null,
                            Status = StatusCodes.Status400BadRequest
                        };

                }
                else
                {
                    res = await uow.IslamRecognition.FindAsync(model.ID);
                    if (res != null)
                    {
                        res.Title = model.Title;
                    }
                    else
                    {
                        return new GenericResult<IsslamRecognition>()
                        {
                            Data = null,
                            Message = null,
                            Status = StatusCodes.Status400BadRequest
                        };
                    }
                }
                uow.SaveChanges();
                return new GenericResult<IsslamRecognition>()
                {
                    Data = res,
                    Message = null,
                    Status = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new GenericResult<IsslamRecognition>()
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
                var res = await uow.IslamRecognition.FindAsync(id);
                if (res != null)
                {
                    uow.IslamRecognition.Remove(res);
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

    }
}
