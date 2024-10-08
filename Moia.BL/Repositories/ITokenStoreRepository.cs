﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moia.DoL.Enums;
using Moia.Shared.Helpers;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;
using Newtonsoft.Json;
using SmsIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Moia.BL.Repositories
{
    public interface ITokenStoreRepository
    {
        Task AddUserTokenAsync(UserToken userToken);
        Task AddUserTokenAsync(MainUser user, string refreshToken, string accessToken, string refreshTokenSource, int ApplicationType);
        Task<bool> IsValidTokenAsync(string accessToken, int userId);
        Task DeleteExpiredTokensAsync();
        Task<UserToken> FindTokenAsync(string refreshToken);
        Task DeleteTokenAsync(string refreshToken);
        Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource);
        Task InvalidateUserTokensAsync(int userId);
        Task InvalidateUserTokensAsync(int userId, int UserokenId);
        Task<(string accessToken, string refreshToken, IEnumerable<Claim> Claims, int Count)> CreateJwtTokens(MainUser user, int applicationType, string refreshTokenSource);
        Task RevokeUserBearerTokensAsync(string userIdValue, string refreshToken);
        Task DeletUserTokensAsync(int userId, int applicationType);
        Task<(string AccessToken, IEnumerable<Claim> Claims, int count)> createAccessTokenAsync(MainUser user);
        string CheckOTP(string username, string otp);
        void SendSMS(string mobile, string code);
    }

    public class TokenStoreRepository : ITokenStoreRepository
    {
        private readonly ISecurityRepository _securityService;
        private readonly IUnitOfWork _uow;
        private readonly IUserTokenRepository _tokens;
        private readonly IUserRepository _users;
        private readonly IOptionsSnapshot<BearerTokensOptions> _configuration;
        private readonly ISessionServices _SessionServices;
        private readonly IDataProtectRepository _dataProtectService;
        public TokenStoreRepository(
            IUnitOfWork uow
            )
        {
            _uow = uow;
            _securityService = uow.SecurityRepository;
            _tokens = uow.UserTokenRepository;
            _users = _uow.User;
            _configuration = uow.OptionsSnapshot;
            _SessionServices = uow.SessionServices;
            _dataProtectService = uow.DataProtectRepository;
        }

        public async Task AddUserTokenAsync(UserToken userToken)
        {
            if (!_configuration.Value.AllowMultipleLoginsFromTheSameUser)
            {
                await InvalidateUserTokensAsync(userToken.UserId);
            }
            await DeleteTokensWithSameRefreshTokenSourceAsync(userToken.RefreshTokenIdHashSource);
            var _userToken = _tokens.AddRange(new List<UserToken> { userToken });
            _SessionServices.UserTokenId = _userToken.ToList().FirstOrDefault().ID.ToString();
            _uow.SaveChanges();
        }

        public async Task AddUserTokenAsync(MainUser user, string refreshToken, string accessToken, string refreshTokenSource, int ApplicationType)
        {
            //  var context = SignalRHubConnectionHandler.Connections.
            var now = DateTimeOffset.UtcNow;
            var token = new UserToken
            {
                UserId = user.ID,
                // Refresh token handles should be treated as secrets and should be stored hashed
                RefreshTokenIdHash = _securityService.GetSha256Hash(refreshToken),
                RefreshTokenIdHashSource = string.IsNullOrWhiteSpace(refreshTokenSource) ?
                                           null : _securityService.GetSha256Hash(refreshTokenSource),
                AccessTokenHash = _securityService.GetSha256Hash(accessToken),
                RefreshTokenExpiresDateTime = now.AddMinutes(double.Parse(_uow.Configuration.GetSection("BearerTokens:RefreshTokenExpirationMinutes").Value)),
                AccessTokenExpiresDateTime = now.AddMinutes(double.Parse(_uow.Configuration.GetSection("BearerTokens:AccessTokenExpirationMinutes").Value)),
                ApplicationType = ApplicationType,
            };
            await AddUserTokenAsync(token);
        }

        public async Task DeleteExpiredTokensAsync()
        {
            var now = DateTimeOffset.UtcNow;
            var ExpiredTokens = _tokens.DbSet.Where(x => x.RefreshTokenExpiresDateTime < now);
            if (ExpiredTokens != null && ExpiredTokens.Count() > 0)
                _tokens.RemoveRange(ExpiredTokens);
            _uow.SaveChanges();
        }

        public async Task DeleteTokenAsync(string refreshToken)
        {
            var token = await FindTokenAsync(refreshToken);
            if (token != null)
            {
                _tokens.RemoveRange(new List<UserToken> { token });
            }
            _uow.SaveChanges();
        }

        public void SendSMS(string mobile, string code)
        {
            if (!string.IsNullOrEmpty(mobile))
            {
                SMSTemplate SMSTemplate_AcceptIndividualTransaction = new SMSTemplate
                {
                    IsActive = true,
                    SMScode = mobile,
                    TextMessage = ""
                };

                SMS_Text_Params_DTO SMSTemplateDTO = new SMS_Text_Params_DTO
                {
                    TextMessage = SMSTemplate_AcceptIndividualTransaction.TextMessage,
                    ParamtersString = SMSTemplate_AcceptIndividualTransaction.Parameters,
                    TemplateCode = SMSTemplate_AcceptIndividualTransaction.TemplateCode,
                    IsActive = SMSTemplate_AcceptIndividualTransaction.IsActive
                };

                SMSDelegationFieldsDTO Fields = new SMSDelegationFieldsDTO()
                {
                    TransactionNumberFormatted = null,
                    From = _SessionServices.OrganizationNameAr,
                    Date = DateTime.UtcNow,
                };

                List<string> FackParam = new List<string>() { "1", "2" };

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    if (SMSTemplateDTO.IsActive)
                    {
                        _uow.SmsIntegrations.Send(mobile, FackParam.ToArray(), $"OTP Code : {code}", SMSTemplateDTO.TemplateCode);
                    }
                }).Start();

            }

        }

        public string CheckOTP(string username, string otp)
        {
            var user = _uow.DbContext.MainUsers.Where(x => x.UserName == username).FirstOrDefault();
            if (user != null)
            {
                if (user.TryloginCount > 4)
                    return EncryptHelper.EncryptString("LockAccount");
                if (user.OTPCreationTime != null)
                {
                    var currentTime = DateTime.UtcNow;
                    var otpTime = currentTime - user.OTPCreationTime.Value;

                    if (otpTime.TotalMinutes > 2)
                    {
                        user.OTP = null;
                        _uow.SaveChanges();
                        return EncryptHelper.EncryptString(false.ToString());
                    }
                    else
                    {
                        if (EncryptHelper.DecryptString(user.OTP) == otp) return EncryptHelper.EncryptString(true.ToString());
                        else
                        {
                            if (user.TryloginCount == null) user.TryloginCount = 0;
                            user.TryloginCount += 1;
                            _uow.SaveChanges();
                            return EncryptHelper.EncryptString(false.ToString());
                        }
                    }

                }

            }
            return EncryptHelper.Encrypt(false.ToString());
        }

        public async Task DeleteTokensWithSameRefreshTokenSourceAsync(string refreshTokenIdHashSource)
        {
            if (string.IsNullOrWhiteSpace(refreshTokenIdHashSource))
            {
                return;
            }
            var ToBeDeletedTokens = _tokens.DbSet.Where(t => t.RefreshTokenIdHashSource == refreshTokenIdHashSource).ToList();
            // check if Its null
            if (ToBeDeletedTokens.Count == 0)
            {
                //check if It _SessionServices.UserTokenId not = 0 if =0 will Delete All _sessions of  this User
                if (int.Parse(_SessionServices.UserTokenId) == 0)
                {
                    await InvalidateUserTokensAsync(_SessionServices.UserId.Value);
                }
                else
                {
                    await InvalidateUserTokensAsync(_SessionServices.UserId.Value, int.Parse(_SessionServices.UserTokenId));

                }
            }
            else
            {
                //(ToBeDeletedTokens != null && ToBeDeletedTokens.Count() > 0)
                _tokens.RemoveRange(ToBeDeletedTokens);
            }
            _uow.SaveChanges();
        }

        public async Task RevokeUserBearerTokensAsync(string userIdValue, string refreshToken)
        {
            if (!string.IsNullOrWhiteSpace(userIdValue) && int.TryParse(userIdValue, out int userId))
            {
                if (_configuration.Value.AllowSignoutAllUserActiveClients)
                {
                    await InvalidateUserTokensAsync(userId);
                }
            }

            if (!string.IsNullOrWhiteSpace(refreshToken))
            {
                var refreshTokenIdHashSource = _securityService.GetSha256Hash(refreshToken);
                await DeleteTokensWithSameRefreshTokenSourceAsync(refreshTokenIdHashSource);
            }
            await DeleteExpiredTokensAsync();
            _uow.SaveChanges();
        }

        public async Task<UserToken> FindTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return null;
            }
            var refreshTokenIdHash = _securityService.GetSha256Hash(refreshToken);
            return await _tokens.DbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshTokenIdHash == refreshTokenIdHash);
        }

        public async Task InvalidateUserTokensAsync(int userId)
        {
            var UserTokens = _tokens.DbSet.Where(x => x.UserId == userId);
            if (UserTokens != null && UserTokens.Count() > 0)
                _tokens.RemoveRange(UserTokens);
            _uow.SaveChanges();
        }
        public async Task InvalidateUserTokensAsync(int userId, int UserokenId)
        {
            var UserTokens = _tokens.DbSet.Where(x => x.UserId == userId && x.ID == UserokenId);
            if (UserTokens != null && UserTokens.Count() > 0)
                _tokens.RemoveRange(UserTokens);
            _uow.SaveChanges();
        }

        public async Task<bool> IsValidTokenAsync(string accessToken, int userId)
        {
            var accessTokenHash = _securityService.GetSha256Hash(accessToken);
            var userToken = await _tokens.DbSet.FirstOrDefaultAsync(
                x => x.AccessTokenHash == accessTokenHash && x.UserId == userId);
            return userToken?.AccessTokenExpiresDateTime >= DateTimeOffset.UtcNow;
        }
        public async Task DeletUserTokensAsync(int userId, int applicationType)
        {
            var UserTokens = _tokens.DbSet.Where(x => x.UserId == userId && x.ApplicationType == applicationType);
            if (UserTokens != null && UserTokens.Count() > 0)
                _tokens.RemoveRange(UserTokens);
            _uow.SaveChanges();
        }

        public async Task<(string accessToken, string refreshToken, IEnumerable<Claim> Claims, int Count)> CreateJwtTokens(MainUser user, int applicationType, string refreshTokenSource)
        {
            var result = await createAccessTokenAsync(user);
            if (result.Claims == null)
                return (null, null, null, 0);
            var refreshToken = Guid.NewGuid().ToString().Replace("-", "");

            //if (_uow.GetRepository<SystemSetting>().GetAll().Where(x => x.SystemSettingCode == "OneUserPerSeesion").SingleOrDefault().SystemSettingValue == "1")
            //{
            //    await DeletUserTokensAsync(user.Id, applicationType);
            //}

            await AddUserTokenAsync(user, refreshToken, result.AccessToken, refreshTokenSource, applicationType);

            _uow.SaveChanges();



            return (result.AccessToken, refreshToken, result.Claims, result.count);
        }

        public async Task<(string AccessToken, IEnumerable<Claim> Claims, int count)> createAccessTokenAsync(MainUser user)
        {
            try
            {
                CommitteeDto committeeDto = null;
                DepartmentDto departmentDto = null;
                MinistryBranshDto ministryBranshDto = null;
                UserDto userDto = null;
                List<string> UserRole = null;
                int Count = 0;
                string _Role = null;
                #region Get User Role Permissions
                var userRoles = _uow.DbContext.MainUserRole.
                                                Include(x => x.Role).
                                                Where(x => x.UserId == user.ID).ToList();
                var userRole = userRoles?.FirstOrDefault();

                if (_uow.DbContext.BranchNegoiationUsers.AsNoTracking().Any(x => x.UserId == user.ID) == true) _Role = UserType.NegoiatedBranchManager.ToString();
                else if (_uow.DbContext.DepartmentNegoiationUsers.AsNoTracking().Any(x => x.UserId == user.ID)) _Role = UserType.NegoiatedDepartmentManager.ToString();

                if (userRole != null || _Role != null)
                {
                    if (_Role == null) _Role = userRole.Role.Code;
                    UserRole = new();
                    UserRole.Add(_Role);
                    if (userRole != null && userRole.CommitteeId != null)
                    {
                        var committee = _uow.Committee.DbSet.Include(x => x.Department).
                            FirstOrDefault(x => x.ID == userRole.CommitteeId);
                        if (committee != null) committeeDto = new CommitteeDto { ID = committee.ID, Title = committee.Title };
                    }
                    else if (userRole != null && userRole.DepartmentId != null)
                    {
                        var dept = _uow.DbContext.Department.FirstOrDefault(x => x.ID == userRole.DepartmentId);
                        if (dept != null) departmentDto = new DepartmentDto
                        {
                            ID = dept.ID,
                            Title = dept.Title
                        };
                    }
                    else if (userRole != null && userRole.BranshId != null)
                    {
                        var Bransh = _uow.DbContext.MinistryBranshs.FirstOrDefault(x => x.ID == userRole.BranshId);
                        if (Bransh != null) ministryBranshDto = new MinistryBranshDto
                        {
                            ID = Bransh.ID,
                            Title = Bransh.Title,
                        };
                    }
                    else { }



                    var User = _uow.User.DbSet.Include(x => x.MainUserRole).ThenInclude(x => x.Role).Include(x=>x.Bransh).AsNoTracking().FirstOrDefault(x => x.ID == user.ID);
                    if (User != null) userDto = new UserDto
                    {
                        ID = user.ID,
                        ActiveDirectoryUser = user.ActiveDirectoryUser,
                        Email = user.Email,
                        Identity = user.Identity,
                        Mobile = user.Mobile,
                        Name = user.Name,
                        UserName = user.UserName,
                        BranchId = user.BranchId,
                        RoleId = User.MainUserRole.FirstOrDefault()?.RoleId,
                        Role = new MainRoleNameId
                        {
                            ID = User.MainUserRole.FirstOrDefault() != null ? User.MainUserRole.FirstOrDefault().ID : 0,
                            Name = User.MainUserRole.FirstOrDefault()?.Role?.Code,
                            NameAr = User.MainUserRole.FirstOrDefault()?.Role?.NameAr
                        },
                        Branch = new BranshDto
                        {
                            ID = user.BranchId,
                            Title = User.Bransh?.Title,
                        }
                    };


                    #endregion

                    var claims = new List<Claim>
                {
                // Unique Id for all Jwt tokes
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                new Claim(ClaimTypes.Role,EncryptHelper.ShiftString(EncryptHelper.EncryptString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(_Role))),4), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                // Issuer
                new Claim(JwtRegisteredClaimNames.Iss, _uow.Configuration.GetSection("BearerTokens:Issuer").Value, ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                new Claim(ClaimTypes.NameIdentifier, EncryptHelper.EncryptString(user.ID.ToString()), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                //new Claim(ClaimTypes.Name,_dataProtectService.Encrypt(user.Username), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                new Claim("KkxR12WKVQOVCuvoJ5vZ3yOrGJZa8GaUcZbgokT4uPM",EncryptHelper.ShiftString(EncryptHelper.EncryptString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(userDto))),4), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                new Claim("NNPO12WKVQOVCuvoJ5vZ3yOrGJkia8GaUcZbgokT4uPM",EncryptHelper.ShiftString(EncryptHelper.EncryptString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(userDto.Name))),4), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                new Claim("NNPO1QQQ2FFFWKVQOVCuvRJ5vZu3yOrTGJkia8GaUOcZbgokTN4uPM",EncryptHelper.ShiftString(EncryptHelper.EncryptString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(UserRole))),9), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value),
                // custom data
                new Claim(ClaimTypes.UserData, user.ID.ToString(), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value)};

                    if (committeeDto != null)
                        claims.Add(new Claim("KkxR12WKVQOVCuvoJ5vZ3yOrhWera8GaUcPMJokT4uPM", EncryptHelper.ShiftString(EncryptHelper.EncryptString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(committeeDto))), 4), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value));

                    if (departmentDto != null)
                        claims.Add(new Claim("KkxR12WKVUPLCuvoJ5vZ3yOrhMHa8GaUQXNgokT4uPM", EncryptHelper.ShiftString(EncryptHelper.EncryptString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(departmentDto))), 4), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value));

                    if (ministryBranshDto != null)
                        claims.Add(new Claim("KkxR12WKVQ902voJ5vZ3yOrhMHaTBGZbgokT4uPM", EncryptHelper.ShiftString(EncryptHelper.EncryptString(EncryptHelper.EncryptString(JsonConvert.SerializeObject(ministryBranshDto))), 4), ClaimValueTypes.String, _uow.Configuration.GetSection("BearerTokens:Issuer").Value));


                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_uow.Configuration.GetSection("BearerTokens:Key").Value));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
                    var now = DateTime.UtcNow;
                    var token = new JwtSecurityToken(
                        issuer: _uow.Configuration.GetSection("BearerTokens:Issuer").Value,
                        audience: _uow.Configuration.GetSection("BearerTokens:Audience").Value,
                        claims: claims,
                        notBefore: now,
                        expires: now.AddMinutes(double.Parse(_uow.Configuration.GetSection("BearerTokens:AccessTokenExpirationMinutes").Value)),
                        signingCredentials: creds);
                    return (new JwtSecurityTokenHandler().WriteToken(token), claims, Count);

                }
                return (null, null, 0);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

    }//MoiaConsoleapp#123

}
