using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moia.BL;
using Moia.BL.Repositories;
using Moia.DoL.Enums;
using Moia.Shared;
using Moia.Shared.Encryption;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Moia.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
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

        [HttpGet("GetByIdCustome")]
        public virtual UserDto GetByIdCustome(int id)
        {
            var user = unitOfWork.User.DbSet.Include(x => x.Bransh).
                                             Include(x => x.MainUserRole).
                                             ThenInclude(x => x.Role).
                                             Include(x => x.Attachment).
                                             AsNoTracking().
                                             FirstOrDefault(x => x.ID == id);
            var res = new UserDto()
            {
                ID = id,
                Name = user.Name,
                Mobile = user.Mobile,
                Email = user.Email,
                Identity = user.Identity,
                ActiveDirectoryUser = user.ActiveDirectoryUser,
                PasswordHash = EncryptHelper.Decrypt(user.PasswordHash),
                UserName = user.UserName,
                BranchId = user.BranchId,
                CommitteeId = user.MainUserRole.FirstOrDefault()?.CommitteeId,
                AttachmentBase64 = user.Attachment?.AttachmentBase64,
                Branch = new BranshDto
                {
                    ID = user.Bransh?.ID,
                    Title = user.Bransh?.Title
                },
                Role = user.MainUserRole.FirstOrDefault()?.RoleId != null ? new MainRoleNameId
                {
                    ID = (int)user.MainUserRole.FirstOrDefault().RoleId,
                    NameAr = user.MainUserRole.FirstOrDefault().Role.NameAr,
                    Name = user.MainUserRole.FirstOrDefault().Role.Code
                } : null,
            };
            return res;
        }

        [HttpPost]
        [Route("GetAllCustom")]
        [ProducesResponseType(typeof(ViewerPagination<MainUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCustom([FromBody] FilterData filterData)
        {
            try
            {
                var myList = unitOfWork.User.getWithPaginate(filterData);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("getWithPaginateByRole")]
        [ProducesResponseType(typeof(ViewerPagination<MainUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> getWithPaginateByRole([FromBody] FilterData filterData)
        {
            try
            {
                var myList = unitOfWork.User.getWithPaginateByRole(filterData);
                return Ok(myList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginModel model)
        {
            try
            {
                //string wwwrootPath = Path.Combine(unitOfWork.ContentRootPath, "wwwroot\\logFile");
                //if (!Directory.Exists(wwwrootPath)) Directory.CreateDirectory(wwwrootPath);
                //string logFilePath = Path.Combine(wwwrootPath, "logFile.txt");
                //System.IO.File.AppendAllText(logFilePath, "Login start api ");

                var res = await unitOfWork.User.DbSet.FirstOrDefaultAsync(x => ((x.UserName == model.Username && x.ActiveDirectoryUser) ||
                                                                            (x.Identity == model.Username && !x.ActiveDirectoryUser) ||
                                                                            (x.UserName == model.Username && x.UserType == UserType.SuperAdmin)));

                if (res != null)
                {
                    if (res.TryloginCount > 4 || !res.Active)
                    {
                        res.Active = false;
                        unitOfWork.SaveChanges();

                        return Ok(EncryptHelper.ShiftString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(new LoginResult
                        {
                            Token = null,
                            refreshToken = null,
                            Message = "LockAccount"
                        })), 6));
                    }
                }

                //System.IO.File.AppendAllText(logFilePath, "Login start api  before unitOfWork.User.Login");
                var result = await unitOfWork.User.Login(model);
                if (result.Status == 200 && result.Message == null)
                {
                    if (res != null)
                    {
                        if (res.TryloginCount == null) res.TryloginCount = 0;
                        res.TryloginCount = 0;
                        unitOfWork.SaveChanges();

                        #region Generate OTP 
                        string OTP = unitOfWork.DbContext.Settings.FirstOrDefault(x => x.Code == "OTP")?.Value;
                        if (OTP == "Enable")
                        {
                            Random random = new Random();
                            string otp = random.Next(1000, 9999).ToString();
                            res.OTP = EncryptHelper.EncryptString(otp);
                            res.OTPCreationTime = DateTime.UtcNow;
                            Console.WriteLine(otp);
                            if (!string.IsNullOrEmpty(res.Mobile)) unitOfWork.TokenStoreRepository.SendSMS(res.Mobile, otp);
                            unitOfWork.SaveChanges();
                        }
                        #endregion

                    }

                    var e = Ok(new LoginResult
                    {
                        Token = result.Token,
                        refreshToken = result.refreshToken,
                        Message = null,
                    });

                    return Ok(EncryptHelper.ShiftString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(new LoginResult
                    {
                        Token = result.Token,
                        refreshToken = result.refreshToken,
                        Message = null,
                        Passchanged = res.PasswordChanged,
                        IsActiveDirectoy = res.ActiveDirectoryUser
                    })), 6));
                }
                else if (result.Status == 400 && result.Message != null)
                {
                    if (res != null)
                    {
                        if (res.TryloginCount == null) res.TryloginCount = 0;
                        res.TryloginCount += 1;
                        unitOfWork.SaveChanges();
                    }



                    return Ok(EncryptHelper.ShiftString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(new LoginResult
                    {
                        Token = null,
                        refreshToken = null,
                        Message = result.Message
                    })), 6));
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }


        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<string> CheckOTP(string username, string otp)
        {
            return unitOfWork.TokenStoreRepository.CheckOTP(username, otp);
        }


        [AllowAnonymous]
        [HttpPost("ChangePassword")]
        public async Task<bool> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                var result = await unitOfWork.User.ChangePassword(model);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //[AllowAnonymous]
        //[HttpPost("ResetPassword")]
        //public async Task<bool> ResetPassword([FromBody] ChangePasswordModel model)
        //{
        //    try
        //    {
        //        var result = await unitOfWork.User.ResetPassword(model);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}



        [AllowAnonymous]
        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(AccessToken))]
        public async Task<IActionResult> RefreshToken([FromBody] JToken jsonBody)
        {
            string refreshToken = jsonBody.Value<string>("refreshToken");
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("refreshToken is not set.");
            }

            var token = await unitOfWork.TokenStoreRepository.FindTokenAsync(refreshToken);
            if (token == null)
            {
                return Unauthorized();
            }
            int applicationType = int.Parse(unitOfWork.SessionServices.ApplicationType);

            var (accessToken, newRefreshToken, claims, count) = await unitOfWork.TokenStoreRepository.CreateJwtTokens(token.User, applicationType, refreshToken);
            //_antiforgery.RegenerateAntiForgeryCookies(claims);
            return Ok(new AccessToken { access_token = accessToken, refresh_token = newRefreshToken });
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> Logout(string refreshToken)
        {
            try
            {
                ClaimsIdentity claimsIdentity = this.User.Identity as ClaimsIdentity;
                string userIdValue = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;

                //string name = User.Identity.Name;

                // The Jwt implementation does not support "revoke OAuth token" (logout) by design.
                // Delete the user's tokens from the database (revoke its bearer token)
                await unitOfWork.TokenStoreRepository.RevokeUserBearerTokensAsync(userIdValue, refreshToken);
                string[] ExecptParm = new string[] { };
                unitOfWork.SessionServices.ClearSessionsExcept(ExecptParm);
                // _antiforgery.DeleteAntiForgeryCookies();
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(AddUserResponse))]
        public virtual async Task<string> AddUpdateUser([FromForm] UserDto model)
        {
            try
            {
                if (model.ID == 0)
                {
                    //|| x.Mobile == model.Mobile || x.Email == model.Email
                    if (!unitOfWork.DbContext.MainUsers.AsNoTracking().Any(x => x.UserName == model.UserName || x.Identity == model.Identity))
                    {
                        int? bi = null;
                        if (model.BranchId == null)
                        {
                            bi = unitOfWork.DbContext.MainUsers.FirstOrDefault(x => x.ID == unitOfWork.SessionServices.UserId)?.BranchId;
                            if (bi != null)
                                model.BranchId = unitOfWork.DbContext.MinistryBranshs.Select(x => new BranshDto { ID = x.ID, Title = x.Title }).FirstOrDefault(x => x.ID == int.Parse(bi.ToString()))?.ID;
                        }

                        if (model.BranchId != null)
                        {
                            MainUser mainUser = new MainUser()
                            {
                                Name = model.Name,
                                ActiveDirectoryUser = model.ActiveDirectoryUser,
                                Email = model.Email != "null" ? model.Email : null,
                                Identity = model.Identity,
                                Mobile = model.Mobile != "null" ? model.Mobile : null,
                                PasswordHash = EncryptHelper.Encrypt(model.PasswordHash),
                                UserName = model.UserName,
                                BranchId = int.Parse(model.BranchId.ToString()),
                                Active = true
                            };
                            var user = unitOfWork.User.Add(mainUser);
                            unitOfWork.SaveChanges();

                            var role = model.RoleId != null ? unitOfWork.DbContext.MainRoles.AsNoTracking().FirstOrDefault(x => x.ID == model.RoleId) : null;

                            if (role != null)
                            {
                                switch (role.Code)
                                {
                                    case nameof(UserType.BranchManager):
                                        unitOfWork.DbContext.MainUserRole.Add(new MainUserRole
                                        {
                                            UserId = user.ID,
                                            RoleId = role.ID,
                                            BranshId = user.BranchId
                                        });

                                        if (model.Signature != null)
                                            using (var imageStream = model.Signature.OpenReadStream())
                                            {
                                                var originalImageStream = new MemoryStream();
                                                await imageStream.CopyToAsync(originalImageStream);

                                                user.Attachment = new Attachment
                                                {
                                                    AttachmentValue = originalImageStream.ToArray(),
                                                    AttachmentBase64 = $"data:image/png;base64,{Convert.ToBase64String(originalImageStream.ToArray())}",
                                                    ImageType = ImageType.Signature
                                                };
                                            }

                                        break;

                                    case nameof(UserType.BranchDataEntry):
                                        unitOfWork.DbContext.MainUserRole.Add(new MainUserRole
                                        {
                                            UserId = user.ID,
                                            RoleId = role.ID,
                                            BranshId = user.BranchId
                                        });
                                        break;
                                    case nameof(UserType.DepartmentManager):
                                        unitOfWork.DbContext.MainUserRole.Add(new MainUserRole
                                        {
                                            UserId = user.ID,
                                            RoleId = role.ID,
                                            DepartmentId = unitOfWork.DbContext.MinistryBranshs.Include(x => x.Department).AsNoTracking().FirstOrDefault(x => x.ID == model.BranchId).Department.ID
                                        });
                                        break;
                                    case nameof(UserType.CommitteeManager):
                                        unitOfWork.DbContext.MainUserRole.Add(new MainUserRole
                                        {
                                            UserId = user.ID,
                                            RoleId = role.ID,
                                            CommitteeId = model.CommitteeId
                                        });
                                        break;

                                    case nameof(UserType.DataEntry):
                                        unitOfWork.DbContext.MainUserRole.Add(new MainUserRole
                                        {
                                            UserId = user.ID,
                                            RoleId = role.ID,
                                            CommitteeId = model.CommitteeId
                                        });
                                        break;
                                }
                                unitOfWork.SaveChanges();
                            }
                            if (user != null)
                                return user.ID.ToString();
                        }
                    }
                }
                else
                {
                    var user = unitOfWork.User.DbSet.Include(x => x.MainUserRole).FirstOrDefault(x => x.ID == model.ID);
                    if (user != null)
                    {
                        //|| x.Mobile == model.Mobile || x.Email == model.Email
                        if (!unitOfWork.DbContext.MainUsers.AsNoTracking().Any(x => (x.UserName == model.UserName || x.Identity == model.Identity) && x.ID != user.ID))
                        {
                            //user.ActiveDirectoryUser = model.ActiveDirectoryUser;
                            user.Identity = model.Identity;
                            user.Email = model.Email != "null" ? model.Email : "";
                            user.UserName = model.UserName;
                            user.Mobile = model.Mobile != "null" ? model.Mobile : "";
                            user.BranchId = model.BranchId;
                            if (model.PasswordHash != null)
                                user.PasswordHash = EncryptHelper.Encrypt(model.PasswordHash);
                            user.Name = model.Name;

                            var role = model.RoleId != null ? unitOfWork.DbContext.MainRoles.AsNoTracking().FirstOrDefault(x => x.ID == model.RoleId) : null;

                            if (role == null)
                            {
                                //user.MainUserRole.Clear();
                                user.MainUserRole = new List<MainUserRole>();

                            }
                            //else
                            //{

                            if (user.MainUserRole == null || user.MainUserRole.Count() == 0) user.MainUserRole = new List<MainUserRole>();
                            //user.MainUserRole.Clear();
                            MainUserRole UserRole;
                            switch (role.Code)
                            {
                                case nameof(UserType.BranchManager):
                                    UserRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.RoleId == role.ID && x.BranshId == user.BranchId);
                                    if (UserRole != null)
                                    {
                                        foreach (var item in user.MainUserRole)
                                        {
                                            if (UserRole.ID != item.ID)
                                                unitOfWork.DbContext.MainUserRole.Remove(item);
                                        }
                                        if (UserRole.UserId != user.ID)
                                        {
                                            user.MainUserRole = new List<MainUserRole>();
                                            UserRole.UserId = user.ID;
                                        }
                                    }
                                    else
                                    {
                                        user.MainUserRole = new List<MainUserRole>();
                                        user.MainUserRole.Add(new MainUserRole
                                        {
                                            RoleId = model.RoleId,
                                            BranshId = model.BranchId,
                                        });
                                    }
                                    if (model.Signature != null)
                                        using (var imageStream = model.Signature.OpenReadStream())
                                        {
                                            var originalImageStream = new MemoryStream();
                                            await imageStream.CopyToAsync(originalImageStream);

                                            user.Attachment = new Attachment
                                            {
                                                AttachmentValue = originalImageStream.ToArray(),
                                                ImageType = ImageType.Signature,
                                                AttachmentBase64 = $"data:image/png;base64,{Convert.ToBase64String(originalImageStream.ToArray())}",
                                            };
                                        }
                                    break;
                                case nameof(UserType.BranchDataEntry):
                                    UserRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.RoleId == role.ID && x.BranshId == user.BranchId);
                                    if (UserRole != null)
                                    {
                                        foreach (var item in user.MainUserRole)
                                        {
                                            if (UserRole.ID != item.ID)
                                                unitOfWork.DbContext.MainUserRole.Remove(item);
                                        }
                                        if (UserRole.UserId != user.ID)
                                        {
                                            user.MainUserRole = new List<MainUserRole>();
                                            UserRole.UserId = user.ID;
                                        }
                                    }
                                    else
                                    {
                                        user.MainUserRole = new List<MainUserRole>();
                                        user.MainUserRole.Add(new MainUserRole
                                        {
                                            RoleId = model.RoleId,
                                            BranshId = model.BranchId,
                                        });
                                    }
                                    break;
                                case nameof(UserType.DepartmentManager):
                                    var deptId = unitOfWork.DbContext.MinistryBranshs.Include(x => x.Department).AsNoTracking().FirstOrDefault(x => x.ID == model.BranchId).Department.ID;
                                    UserRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.RoleId == role.ID && x.DepartmentId == deptId);
                                    if (UserRole != null)
                                    {
                                        foreach (var item in user.MainUserRole)
                                        {
                                            if (UserRole.ID != item.ID)
                                                unitOfWork.DbContext.MainUserRole.Remove(item);
                                        }
                                        if (UserRole.UserId != user.ID)
                                        {
                                            user.MainUserRole = new List<MainUserRole>();
                                            UserRole.UserId = user.ID;
                                        }
                                    }
                                    else
                                    {
                                        user.MainUserRole = new List<MainUserRole>();
                                        user.MainUserRole.Add(new MainUserRole
                                        {
                                            RoleId = model.RoleId,
                                            DepartmentId = deptId
                                        });
                                    }
                                    break;
                                case nameof(UserType.CommitteeManager):
                                    UserRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.RoleId == role.ID && x.CommitteeId == model.CommitteeId);
                                    if (UserRole != null)
                                    {

                                        foreach (var item in user.MainUserRole)
                                        {
                                            if (UserRole.ID != item.ID)
                                                unitOfWork.DbContext.MainUserRole.Remove(item);
                                        }
                                        if (UserRole.UserId != user.ID)
                                        {
                                            user.MainUserRole = new List<MainUserRole>();
                                            UserRole.UserId = user.ID;
                                        }
                                    }
                                    else
                                    {
                                        user.MainUserRole = new List<MainUserRole>();
                                        user.MainUserRole.Add(new MainUserRole
                                        {
                                            RoleId = model.RoleId,
                                            CommitteeId = model.CommitteeId,
                                        });
                                    }




                                    break;
                                case nameof(UserType.DataEntry):
                                    UserRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.RoleId == role.ID && x.CommitteeId == model.CommitteeId && x.UserId ==null);
                                    if (UserRole != null)
                                    {
                                        //foreach (var item in user.MainUserRole)
                                        //{
                                        //    if (UserRole.ID != item.ID)
                                        //        unitOfWork.DbContext.MainUserRole.Remove(item);
                                        //}

                                        if (UserRole.UserId != user.ID)
                                        {
                                            user.MainUserRole = new List<MainUserRole>();
                                            UserRole.UserId = user.ID;
                                        }
                                    }
                                    else
                                    {
                                        //if(UserRole !=null)
                                        user.MainUserRole = new List<MainUserRole>();
                                        user.MainUserRole.Add(new MainUserRole
                                        {
                                            RoleId = model.RoleId,
                                            CommitteeId = model.CommitteeId,
                                        });
                                    }

                                    break;
                            }




                            unitOfWork.SaveChanges();
                            return user.ID.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpPost("[action]")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public virtual async Task<bool> SaveNegotiateUser(List<CommitteeRoleViewModel> model)
        {
            try
            {
                var UserRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == unitOfWork.SessionServices.UserId);
                UserType userType = unitOfWork.SessionServices.UserType;
                switch (userType)
                {
                    case UserType.DepartmentManager:
                        var users = unitOfWork.DbContext.DepartmentNegoiationUsers.AsNoTracking().Where(x => x.DepartmentId == UserRole.DepartmentId);
                        if (users != null) unitOfWork.DbContext.DepartmentNegoiationUsers.RemoveRange(users);
                        unitOfWork.SaveChanges();
                        break;

                    case UserType.BranchManager:
                        var _users = unitOfWork.DbContext.BranchNegoiationUsers.AsNoTracking().Where(x => x.BranchId == UserRole.BranshId);
                        if (_users != null) unitOfWork.DbContext.BranchNegoiationUsers.RemoveRange(_users);
                        unitOfWork.SaveChanges();
                        break;
                    default:
                        break;
                }


                if (model != null && model.Count() > 0)
                {
                    foreach (var item in model)
                    {
                        bool UserExist = false;
                        if (userType == UserType.NegoiatedDepartmentManager) UserExist = unitOfWork.DbContext.DepartmentNegoiationUsers.AsNoTracking().Any(x => x.UserId == item.ID);
                        else if (userType == UserType.NegoiatedBranchManager) UserExist = unitOfWork.DbContext.BranchNegoiationUsers.AsNoTracking().Any(x => x.UserId == item.ID);
                        if (!UserExist)
                        {
                            switch (userType)
                            {
                                case UserType.DepartmentManager:
                                    unitOfWork.DbContext.DepartmentNegoiationUsers.Add(new DepartmentNegoiationUsers
                                    {
                                        UserId = int.Parse(item.ID.ToString()),
                                        DepartmentId = int.Parse(UserRole.DepartmentId.ToString())
                                    });
                                    unitOfWork.SaveChanges();
                                    break;

                                case UserType.BranchManager:
                                    unitOfWork.DbContext.BranchNegoiationUsers.Add(new BranchNegoiationUsers
                                    {
                                        UserId = int.Parse(item.ID.ToString()),
                                        BranchId = int.Parse(UserRole.BranshId.ToString())
                                    });
                                    unitOfWork.SaveChanges();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                return true;

            }
            catch (Exception ex)
            {

            }
            return false;
        }



        [HttpGet("getAllADUser")]
        public virtual string getAllADUser()
        {
            return EncryptHelper.Encrypt(JsonConvert.SerializeObject(unitOfWork.Mapper.Map<List<UserDto>>(unitOfWork.User.DbSet.Where(x => x.ActiveDirectoryUser == true))));
        }

        [HttpGet("getNegotiatedUser")]
        public virtual List<CommitteeRoleViewModel> getNegotiatedUser()
        {
            UserType userType = unitOfWork.SessionServices.UserType;
            List<CommitteeRoleViewModel> res = new List<CommitteeRoleViewModel>();
            var users = unitOfWork.DbContext.MainUsers.Where(x => x.ActiveDirectoryUser == true);
            switch (userType)
            {
                case UserType.DepartmentManager:
                    var DepartmentId = unitOfWork.DbContext.MainUserRole.AsNoTracking().
                                        FirstOrDefault(x => x.UserId == unitOfWork.SessionServices.UserId &&
                                                       x.RoleId == unitOfWork.DbContext.MainRoles.FirstOrDefault(x => x.Code == userType.ToString()).ID)?.DepartmentId;

                    var NegoiatedUser = unitOfWork.DbContext.DepartmentNegoiationUsers.AsNoTracking().Where(x => x.DepartmentId == DepartmentId).Select(x => x.UserId);
                    res = users.Where(x => NegoiatedUser.Contains(x.ID)).Select(x => new CommitteeRoleViewModel
                    {
                        ID = x.ID,
                        Title = x.Name
                    }).ToList();

                    break;

                case UserType.BranchManager:
                    var BranchId = unitOfWork.DbContext.MainUserRole.AsNoTracking().
                                        FirstOrDefault(x => x.UserId == unitOfWork.SessionServices.UserId &&
                                                       x.RoleId == unitOfWork.DbContext.MainRoles.FirstOrDefault(x => x.Code == userType.ToString()).ID)?.BranshId;

                    var _NegoiatedUser = unitOfWork.DbContext.BranchNegoiationUsers.AsNoTracking().Where(x => x.BranchId == BranchId).Select(x => x.UserId);
                    res = users.Where(x => _NegoiatedUser.Contains(x.ID)).Select(x => new CommitteeRoleViewModel
                    {
                        ID = x.ID,
                        Title = x.Name
                    }).ToList();
                    break;
                default:
                    break;
            }
            return res;

        }

        [HttpPost("UpdateDeptM")]
        public virtual bool updateDepartmentManager(int userId, int DeptId)
        {
            MainRole mainRole = unitOfWork.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.DepartmentManager.ToString());
            var DepartmentManager = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.DepartmentId == DeptId && x.RoleId == mainRole.ID);
            var userRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == userId);
            if (DepartmentManager != null)
            {
                if (userRole != null) userRole.UserId = null;
                DepartmentManager.UserId = userId;
                unitOfWork.SaveChanges();
                return true;
            }
            else
            {
                if (userRole != null) userRole.UserId = null;
                unitOfWork.DbContext.MainUserRole.Add(new MainUserRole
                {
                    UserId = userId,
                    RoleId = mainRole.ID,
                    DepartmentId = DeptId
                });
                unitOfWork.SaveChanges();
                return true;

            }
        }

        [HttpPost("UpdateBrM")]
        public virtual bool updateBranchManager(int userId, int BranchId)
        {
            MainRole mainRole = unitOfWork.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString());
            var BranchManager = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.BranshId == BranchId && x.RoleId == mainRole.ID);
            var userRole = unitOfWork.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == userId);
            if (BranchManager != null)
            {
                if (userRole != null) userRole.UserId = null;
                BranchManager.UserId = userId;
                unitOfWork.SaveChanges();
                return true;
            }
            else
            {
                if (userRole != null) userRole.UserId = null;
                unitOfWork.DbContext.MainUserRole.Add(new MainUserRole
                {
                    UserId = userId,
                    RoleId = mainRole.ID,
                    BranshId = BranchId
                });
                unitOfWork.SaveChanges();
                return true;

            }
        }


        [HttpPost("InsertUserSignature")]
        public async Task InsertImage()
        {
            try
            {
                var files = Request.Form.Files[0];
                var UserId = unitOfWork.SessionServices.UserId;
                var user = unitOfWork.DbContext.MainUsers.FirstOrDefault(x => x.ID == UserId);
                if (user != null)
                {
                    using (var imageStream = files.OpenReadStream())
                    {
                        var originalImageStream = new MemoryStream();
                        await imageStream.CopyToAsync(originalImageStream);

                        user.Attachment = new Attachment
                        {
                            AttachmentValue = originalImageStream.ToArray(),
                            ImageType = ImageType.Signature
                        };
                        unitOfWork.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


        [HttpPost("ActivateUser")]
        public bool ActivateUser(int UserId, bool state)
        {
            return unitOfWork.User.ActivateUser(UserId, state);
        }



    }

    public class UploadAttachment
    {
        public int ID { get; set; }
        public IFormFile File { get; set; }
    }
}