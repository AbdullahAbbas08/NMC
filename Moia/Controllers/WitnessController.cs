using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WitnessController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        public WitnessController(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        [HttpGet("GetById")]
        public virtual Witness Get(int id)
        {
            try
            {
                var res = uow.Witness.Get().FirstOrDefault(x => x.ID == id);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetAll")]
        public virtual List<WitnessDto> GetAll()
        {
            try
            {
                var res = uow.Witness.DbSet.OrderBy(x=>x.CreatedOn).Select(x => new WitnessDto
                {
                    ID = x.ID,
                    Name = x.Name,
                }).ToList();
                return res;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetAllCustom")]
        [ProducesResponseType(typeof(ViewerPagination<Witness>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult GetAll(string? searchTerm, int page, int pageSize)
        {
            try
            {
                var myList = uow.Witness.getWithPaginate(page, pageSize, searchTerm);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Insert")]
        public async Task<GenericResult<WitnessDto>> Insert(Witness model)
        {
            try
            {
                Witness res = null;
                if (model.ID == 0)
                {
                    if (!uow.DbContext.Witness.Any(x => x.Identity == model.Identity || x.Mobile == model.Mobile))
                    {
                        res = uow.Witness.Add(model);
                    }
                    else
                    {
                        if (res == null)
                            return new GenericResult<WitnessDto>()
                            {
                                Data = null,
                                Message = "رقم الموبايل أو رقم الهوية موجودة بالفعل ",
                                Status = StatusCodes.Status400BadRequest
                            };
                    }
                }
                else
                {
                    res = uow.Witness.FirstOrDefault(x => x.ID == model.ID);
                    if (res != null)
                    {
                        res.Identity = model.Identity;
                        res.Name = model.Name;
                        res.Mobile = model.Mobile;
                    }
                    else
                    {
                        return new GenericResult<WitnessDto>()
                        {
                            Data = null,
                            Message = "الشاهد موجود بالفعل",
                            Status = StatusCodes.Status400BadRequest
                        };
                    }
                }
                uow.SaveChanges();
                return new GenericResult<WitnessDto>()
                {
                    Data = new WitnessDto { ID = res.ID , Name = res.Name},
                    Message = "تم حفظ البيانات بنجاح",
                    Status = StatusCodes.Status200OK
                };
            }
            catch (Exception ex)
            {
                return new GenericResult<WitnessDto>()
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
                var res = await uow.Witness.FindAsync(id);
                if (res != null)
                {
                    uow.Witness.Remove(res);
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
