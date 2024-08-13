using Moia.DoL.Enums;
using Moia.Shared.Helpers;
using Moia.Shared.Models;

namespace Moia.BL.Repositories
{
    public interface IUserRepository : IRepository<MainUser>
    {
        ViewerPagination<MainUserDto> getWithPaginate(FilterData filterData);
        ViewerPagination<MainUserDto> getWithPaginateByRole(FilterData filterData);
        Task<AuthModel> Login(UserLoginModel model);
        Task<MainUser> FindUserPasswordAsync(string username, string password, bool isHashedPassword);
        Task<bool> ChangePassword(ChangePasswordModel model);
        bool ActivateUser(int UserId, bool state);
        //Task<bool> ResetPassword(ChangePasswordModel model);

    }

    public class UserRepository : Repository<MainUser>, IUserRepository
    {
        private readonly IUnitOfWork uow;

        public UserRepository(IUnitOfWork _uow) : base(_uow)
        {
            uow = _uow;
        }

        public async Task<bool> ChangePassword(ChangePasswordModel model)
        {
            var authModel = new AuthModel();
            try
            {
                string HashedPass = EncryptHelper.Encrypt(model.OldPassword);
                MainUser user;
                user = uow.User.DbSet.FirstOrDefault(x => x.Identity == model.Username && x.PasswordHash == HashedPass);
                if (user is null) return false;
                if (user.ActiveDirectoryUser) return false;
                user.PasswordHash = EncryptHelper.Encrypt(model.NewPassword);
                user.PasswordChanged = true;
                uow.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                authModel.Message = ex.Message;
                authModel.IsAuthenticated = false;
                authModel.Status = 500;
            }
            return false;
        }

        //public async Task<bool> ResetPassword(ChangePasswordModel model)
        //{
        //    var authModel = new AuthModel();
        //    try
        //    {
        //        MainUser user;
        //        // local 
        //        if (model.IdentityNo == null) return false;
        //        user = await uow.User.DbSet.FirstOrDefaultAsync(x => x.UserName == model.Username );
        //        if (user is null) return false;
        //        user.PasswordHash = EncryptHelper.Encrypt(model.NewPassword);
        //        uow.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        authModel.Message = ex.Message;
        //        authModel.IsAuthenticated = false;
        //        authModel.Status = 500;
        //    }
        //    return false;
        //}

        public ViewerPagination<MainUserDto> getWithPaginate(FilterData filterData)
        {
            var UserId = uow.SessionServices.UserId;
            var logedInUser = uow.DbContext.MainUsers.AsNoTracking().FirstOrDefault(x => x.ID == UserId);

            IQueryable<MainUser> myData;
            int myDataCount = 0;

            myData = uow.DbContext.MainUsers
                .Include(x => x.Bransh)
                .ThenInclude(x => x.Department)
                .Include(x => x.MainUserRole)
                .ThenInclude(x => x.Role)
                .Where(x => x.Display)
                .Where(x => x.BranchId == logedInUser.BranchId || (logedInUser.BranchId == null && logedInUser.UserType == UserType.SuperAdmin))
                .Where(x => (filterData.brn == null || x.BranchId == filterData.brn) &&
                            (filterData.dept == null || x.Bransh.Department.ID == filterData.dept) &&
                            (filterData.active == 3 || x.Active == (filterData.active == 1 ? true : false)) &&
                            (filterData.com == null || x.MainUserRole.Any(x => x.CommitteeId == filterData.com)) &&
                            (filterData.role == null || x.MainUserRole.Any(x => x.RoleId == filterData.role))
                           )
                .Where(a => filterData.searchTerm == null ||
                            a.Name.Contains(filterData.searchTerm) ||
                            a.UserName.Contains(filterData.searchTerm) ||
                            a.Identity.Contains(filterData.searchTerm) ||
                            a.Mobile.Contains(filterData.searchTerm) ||
                            a.Email.Contains(filterData.searchTerm));

            myDataCount = myData.Count();

            ViewerPagination<MainUserDto> viewerPagination = new ViewerPagination<MainUserDto>
            {
                PaginationList = myData.Skip((filterData.page - 1) * filterData.pageSize)
                                       .Take(filterData.pageSize)
                                       .Select(x => new MainUserDto
                                       {
                                           ID = x.ID,
                                           Name = x.Name,
                                           Email = x.Email,
                                           Identity = x.Identity,
                                           Mobile = x.Mobile,
                                           UserName = x.UserName,
                                           RoleTitle = string.Join(", ", x.MainUserRole.Select(y => y.Role.NameAr)),
                                           Enable = x.Active
                                       }).ToList(),
                OriginalListListCount = myDataCount
            };

            return viewerPagination;
        }


        public ViewerPagination<MainUserDto> getWithPaginateByRole(FilterData filterData)
        {
            string RoleCode = "";
            int RoleId = 0;
            int? committeeId = 0;
            List<string> EntryAndManagerCommitteeMembersCodes = new List<string>() {
            $"{RoleCodes.DataEntry}",
            $"{RoleCodes.CommitteeManager}"
            };

            var EntryAndManagerCommitteeMembers = uow.DbContext.MainUserRole.Where(x => EntryAndManagerCommitteeMembersCodes.Contains( x.Role.Code)).Select(x => x.UserId);
            IQueryable<int?> CommitteeMembers;

            var UserId = uow.SessionServices.UserId;
            var userRole = uow.DbContext.MainUserRole.Include(x => x.Role).FirstOrDefault(x => x.UserId == UserId);
            if (userRole != null)
            {
                RoleCode = userRole.Role.Code;
                RoleId = userRole.Role.ID;
                committeeId = userRole.CommitteeId;
            }
            var logedInUser = uow.DbContext.MainUsers.AsNoTracking().FirstOrDefault(x => x.ID == UserId);

            IQueryable<MainUser> myData;
            int myDataCount = 0;

            switch (RoleCode)
            {
                case RoleCodes.CommitteeManager:
                    CommitteeMembers = uow.DbContext.MainUserRole.Where(x => x.CommitteeId == committeeId).Select(x => x.UserId);
                    myData = uow.DbContext.MainUsers
                                          .Include(x => x.Bransh)
                                          .ThenInclude(x => x.Department)
                                          .Include(x => x.MainUserRole)
                                          .ThenInclude(x => x.Role)
                                          .Where(x => x.Display)
                                          .Where(x => x.BranchId == logedInUser.BranchId || (logedInUser.BranchId == null && logedInUser.UserType == UserType.SuperAdmin))
                                          .Where(x => CommitteeMembers.Contains(x.ID))
                                          .Where(x => (filterData.brn == null || x.BranchId == filterData.brn) &&
                                                      (filterData.dept == null || x.Bransh.Department.ID == filterData.dept) &&
                                                      (filterData.active == 3 || x.Active == (filterData.active == 1 ? true : false)) &&
                                                      (filterData.com == null || x.MainUserRole.Any(x => x.CommitteeId == filterData.com)) &&
                                                      (filterData.role == null || x.MainUserRole.Any(x => x.RoleId == filterData.role))
                                                     )
                                          .Where(a => filterData.searchTerm == null ||
                                                      a.Name.Contains(filterData.searchTerm) ||
                                                      a.UserName.Contains(filterData.searchTerm) ||
                                                      a.Identity.Contains(filterData.searchTerm) ||
                                                      a.Mobile.Contains(filterData.searchTerm) ||
                                                      a.Email.Contains(filterData.searchTerm));
                    break;
                default:
                    myData = uow.DbContext.MainUsers
                                        .Include(x => x.Bransh)
                                        .ThenInclude(x => x.Department)
                                        .Include(x => x.MainUserRole)
                                        .ThenInclude(x => x.Role)
                                        .Where(x => x.Display)
                                        .Where(x => x.BranchId == logedInUser.BranchId || (logedInUser.BranchId == null && logedInUser.UserType == UserType.SuperAdmin))
                                        .Where(x => EntryAndManagerCommitteeMembers.Contains(x.ID))
                                        .Where(x => (filterData.brn == null || x.BranchId == filterData.brn) &&
                                                    (filterData.dept == null || x.Bransh.Department.ID == filterData.dept) &&
                                                    (filterData.active == 3 || x.Active == (filterData.active == 1 ? true : false)) &&
                                                    (filterData.com == null || x.MainUserRole.Any(x => x.CommitteeId == filterData.com)) &&
                                                    (filterData.role == null || x.MainUserRole.Any(x => x.RoleId == filterData.role))
                                                   )
                                        .Where(a => filterData.searchTerm == null ||
                                                    a.Name.Contains(filterData.searchTerm) ||
                                                    a.UserName.Contains(filterData.searchTerm) ||
                                                    a.Identity.Contains(filterData.searchTerm) ||
                                                    a.Mobile.Contains(filterData.searchTerm) ||
                                                    a.Email.Contains(filterData.searchTerm));
                    break;
            }



            myDataCount = myData.Count();

            ViewerPagination<MainUserDto> viewerPagination = new ViewerPagination<MainUserDto>
            {
                PaginationList = myData.Skip((filterData.page - 1) * filterData.pageSize)
                                       .Take(filterData.pageSize)
                                       .Select(x => new MainUserDto
                                       {
                                           ID = x.ID,
                                           Name = x.Name,
                                           Email = x.Email,
                                           Identity = x.Identity,
                                           Mobile = x.Mobile,
                                           UserName = x.UserName,
                                           RoleTitle = string.Join(", ", x.MainUserRole.Select(y => y.Role.NameAr)),
                                           Enable = x.Active
                                       }).ToList(),
                OriginalListListCount = myDataCount
            };

            return viewerPagination;
        }

        public async Task<MainUser> FindUserPasswordAsync(string username, string password, bool isHashedPassword)
        {
            string passwordHash = EncryptHelper.Encrypt(password);
            MainUser result = await uow.User.DbSet.FirstOrDefaultAsync(x => ((x.UserName == username && x.ActiveDirectoryUser) ||
                                                                            (x.Identity == username && !x.ActiveDirectoryUser) ||
                                                                            (x.UserName == username && x.UserType == UserType.SuperAdmin)) &&
                                                                            (x.PasswordHash == passwordHash));
            return result;
        }
        public async Task<AuthModel> Login(UserLoginModel model)
        {
            var authModel = new AuthModel();
            try
            {
                //string wwwrootPath = Path.Combine(uow.ContentRootPath, "wwwroot\\logFile");
                //if (!Directory.Exists(wwwrootPath)) Directory.CreateDirectory(wwwrootPath);
                //string logFilePath = Path.Combine(wwwrootPath, "logFile.txt");
                //File.AppendAllText(logFilePath, "Login start");


                MainUser user = null;

                //Active Directory
                //File.WriteAllText(logFilePath, "before AuthenticatAD");
                user = AuthenticatAD(model.Username, model.Password);
                //File.WriteAllText(logFilePath, "after AuthenticatAD");

                if (user == null)
                {
                    // local 
                    user = await FindUserPasswordAsync(model.Username, model.Password, false);
                }

                if (user is null)
                {
                    authModel.Message = "InvalidUsernameOrPassword";
                    authModel.IsAuthenticated = false;
                    authModel.Status = 400;
                    return authModel;
                }

                var jwtSecurityToken = uow.TokenStoreRepository.CreateJwtTokens(user, 1, null);
                authModel.Message = null;
                authModel.IsAuthenticated = true;
                authModel.Status = 200;
                authModel.Token = jwtSecurityToken.Result.accessToken;
                authModel.refreshToken = jwtSecurityToken.Result.refreshToken;
                authModel.Count = jwtSecurityToken.Result.Count;
            }
            catch (Exception ex)
            {
                authModel.Message = ex.Message;
                authModel.IsAuthenticated = false;
                authModel.Status = 500;
            }
            return authModel;
        }

        public MainUser AuthenticatAD(string userName, string password)
        {
            try
            {
                //string wwwrootPath = Path.Combine(uow.ContentRootPath, "wwwroot\\logFile");
                //string logFilePath = Path.Combine(wwwrootPath, "logFile.txt");
                //File.AppendAllText(logFilePath, "Enter AuthenticatAD");

                string domainIP = EncryptHelper.Decrypt(uow.Configuration["ADSettings:ServerIP"]);
                string domainName = EncryptHelper.Decrypt(uow.Configuration["ADSettings:DomainName"]);
                string domainUserName = EncryptHelper.Decrypt(uow.Configuration["ADSettings:UserName"]);
                string domainPassword = EncryptHelper.Decrypt(uow.Configuration["ADSettings:Password"]);

                PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainIP, domainUserName, domainPassword);
                bool isValid = pc.ValidateCredentials(userName, password);

                if (isValid)
                {
                    //File.AppendAllText(logFilePath, "isValid AuthenticatAD");
                    var UserExist = uow.User.DbSet.FirstOrDefault(x => x.UserName == userName);

                    return UserExist;
                }
                //File.AppendAllText(logFilePath, "isValid not valid AuthenticatAD");
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public bool ActivateUser(int UserId, bool state)
        {
            bool res = false;
            try
            {
                var User = uow.User.DbSet.FirstOrDefault(x => x.ID == UserId);
                if (User != null)
                {
                    if (state)
                    {
                        User.Active = true;
                        User.TryloginCount = 0;
                    }
                    else
                    {
                        User.Active = false;
                    }
                    uow.SaveChanges();
                    res = true;
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }

}
