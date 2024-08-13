using Moia.DoL.Enums;
using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;

namespace Moia.Services
{
    public static partial class ServicesRegistration
    {
        public static void AddDatabaseContext(this IServiceCollection services, string connectionString)
        {

            services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(options =>
            {
                options.UseLazyLoadingProxies(false)
                .UseSqlServer(connectionString, serverDbContextOptionsBuilder =>
                {
                    int minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    serverDbContextOptionsBuilder.EnableRetryOnFailure();
                });
            });
        }

        public static void DatabaseMigration(this IServiceCollection services)
        {
            //IServiceProvider serviceProvider = services.BuildServiceProvider();
        }

        public static async void DatabaseInitialData(this IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IUnitOfWork uow = serviceProvider.GetService<IUnitOfWork>();

            #region Countries Initial
            string path = $"{uow.ContentRootPath}wwwroot\\countries.json";
            var jsonContent = File.ReadAllText(path);
            List<countryFromFile> data = System.Text.Json.JsonSerializer.Deserialize<List<countryFromFile>>(jsonContent);

            if (!uow.DbContext.Countries.Any())
            {
                foreach (var item in data)
                {
                    uow.DbContext.Countries.Add(new Country { NameAr = item.arabic_name, NameEn = item.english_name, Code = item.code });
                }
                uow.SaveChanges();
            }


            #endregion

            #region Cities Initial
            string Cities_Path = $"{uow.ContentRootPath}wwwroot\\cities.json";
            var CitiesJsonContent = File.ReadAllText(Cities_Path);
            CitiesFromFile CitiesData = System.Text.Json.JsonSerializer.Deserialize<CitiesFromFile>(CitiesJsonContent);

            if (!uow.DbContext.ResidenceIssuePlace.Any())
            {
                List< ResidenceIssuePlace > CitiesList = new List< ResidenceIssuePlace >();
                foreach (var item in CitiesData.cities)
                {
                    CitiesList.Add(new ResidenceIssuePlace
                    {
                        Title = item
                    });
                }

                uow.DbContext.ResidenceIssuePlace.AddRange(CitiesList);
                uow.SaveChanges();
            }


            #endregion

            #region Initiate Roles
            if (!uow.DbContext.MainRoles.Any())
            {
                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.SuperAdmin.ToString(),
                    NameAr = UserTypeAr.SuperAdmin.ToString(),
                });

                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.DataEntry.ToString(),
                    NameAr = UserTypeAr.DataEntry.ToString(),
                });

                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.CommitteeManager.ToString(),
                    NameAr = UserTypeAr.CommitteeManager.ToString()
                });

                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.DepartmentManager.ToString(),
                    NameAr = UserTypeAr.DepartmentManager.ToString()
                });

                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.BranchManager.ToString(),
                    NameAr = UserTypeAr.BranchManager.ToString()
                });

                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.NegoiatedDepartmentManager.ToString(),
                    NameAr = UserTypeAr.NegoiatedDepartmentManager.ToString()
                });

                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.NegoiatedBranchManager.ToString(),
                    NameAr = UserTypeAr.NegoiatedBranchManager.ToString()
                });

                await uow.DbContext.MainRoles.AddAsync(new MainRole
                {
                    Code = UserType.BranchDataEntry.ToString(),
                    NameAr = UserTypeAr.BranchDataEntry.ToString()
                });

                uow.SaveChanges();
            }
            #endregion

            #region Initiate Users
            if (!uow.DbContext.MainUsers.Any(x => x.UserName == "DeveloperTeam250") && uow.DbContext.MainRoles.Any())
            {
                var user = new MainUser
                {
                    UserName = "DeveloperTeam250",
                    Name = "Technical Support",
                    PasswordHash = EncryptHelper.Encrypt("MoiaConsoleapp#123"),
                    UserType = UserType.SuperAdmin,
                    Display = false,
                    ActiveDirectoryUser = false,
                    Email = "technicalSupport@tafeel.com",
                    PasswordChanged = true
                };

                await uow.DbContext.MainUsers.AddAsync(user);
                uow.SaveChanges();

                // Extract the user entity after saving changes
                var addedUser = uow.DbContext.Entry(user).Entity;

                if (addedUser.MainUserRole == null) addedUser.MainUserRole = new List<MainUserRole>();

                addedUser.MainUserRole.Add(new MainUserRole
                {
                    Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.SuperAdmin.ToString())
                });
                uow.SaveChanges();
            }
            #endregion

            #region Initiate Branchs and Departments
            if (!uow.DbContext.MinistryBranshs.Any())
            {
                List<MinistryBransh> ministryBranshes = new List<MinistryBransh>() {
                new MinistryBransh {Title = "منطقة مكة المكرمة" , Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة مكة المكرمة" },Code = "001" },
                new MinistryBransh {Title = "منطقة الرياض",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة الرياض" } ,Code = "002"},
                new MinistryBransh {Title = "منطقة  القصيم",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة القصيم" },Code = "003"},
                new MinistryBransh {Title = "منطقة  الحدود الشمالية",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة الحدود الشمالية" }, Code = "004"},
                new MinistryBransh {Title = "منطقة  المدينة المنورة",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة المدينة المنورة" },Code = "005"},
                new MinistryBransh {Title = "المنطقة الشرقية",Department = new Department{Title = "إدارة الدعوة والإرشاد بالمنطقة الشرقية" }, Code = "006"},
                new MinistryBransh {Title = "منطقة  نجران",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة نجران" },Code = "007"},
                new MinistryBransh {Title = "منطقة  الجوف",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة الجوف" }, Code = "008"},
                new MinistryBransh {Title = "منطقة  حائل",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة حائل" },Code = "009"},
                new MinistryBransh {Title = "منطقة  عسير",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة عسير" }, Code = "010"},
                new MinistryBransh {Title = "منطقة  جازان",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة جازان" },Code = "011"},
                new MinistryBransh {Title = "منطقة  تبوك",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة تبوك" }, Code = "012"},
                new MinistryBransh {Title = "منطقة  الباحة",Department = new Department{Title = "إدارة الدعوة والإرشاد بمنطقة الباحة" },Code = "013"},
                };
                await uow.DbContext.MinistryBranshs.AddRangeAsync(ministryBranshes);
                uow.SaveChanges();

            }
            #endregion

            #region Islam Recognition Way
            if (!uow.DbContext.IsslamRecognition.Any())
            {
                List<IsslamRecognition> isslamRecognitions = new List<IsslamRecognition>()
                {
                    new IsslamRecognition{Title="جريدة"},
                    new IsslamRecognition{Title="برنامج تلفيزيونى"},
                    new IsslamRecognition{Title="الإزاعة"},
                    new IsslamRecognition{Title="شريط فيديو"},
                    new IsslamRecognition{Title="شريط مسجل"},
                    new IsslamRecognition{Title="الكمبيوتر وبرامجه"},
                    new IsslamRecognition{Title="المثال الطيب من المسلم"},
                    new IsslamRecognition{Title="عيادات فى الإسلام مثل الصلاة"},
                    new IsslamRecognition{Title="كتيب ومطويه"},
                    new IsslamRecognition{Title="الإنترنت"},
                    new IsslamRecognition{Title="من الكفيل"},
                    new IsslamRecognition{Title="من قريب مسلممن صديق مسلم"},
                    new IsslamRecognition{Title="من محاضرات"},
                    new IsslamRecognition{Title="أخرى"}
                };
                uow.DbContext.IsslamRecognition.AddRange(isslamRecognitions);
                uow.SaveChanges();
            }
            #endregion

            #region Religions 
            if (!uow.DbContext.Religions.Any())
            {
                List<Religion> Religions = new List<Religion>()
                {
                    new Religion{Title = "مسيحى"},
                    new Religion{Title = "يهودى"},
                    new Religion{Title = "بوزى"},
                    new Religion{Title = "هندوسي"},
                    new Religion{Title = "سيخ"},
                    new Religion{Title = "شنتو"},
                    new Religion{Title = "بهائي"},
                    new Religion{Title = "جاينية"},
                    new Religion{Title = "زرادشتية"},
                    new Religion{Title = "كونفوشيوسية"},
                    new Religion{Title = "يانغاويسم"},
                    new Religion{Title = "طائفة اللاترية"},
                    new Religion{Title = "روحانية تقليدية"},
                    new Religion{Title = "أفريكان ترادشنال ريليجينز"},
                };
                uow.DbContext.Religions.AddRange(Religions);
                uow.SaveChanges();
            }
            #endregion

            #region Educational Levels 
            if (!uow.DbContext.EducationalLevels.Any())
            {
                List<EducationalLevel> EducationalLevels = new List<EducationalLevel>()
                {
                    new EducationalLevel{Title = "إبتدائية"},
                    new EducationalLevel{Title = "إعدادية"},
                    new EducationalLevel{Title = "ثانوية"},
                    new EducationalLevel{Title = "تعليم فني"},
                    new EducationalLevel{Title = "تعليم مهني"},
                    new EducationalLevel{Title = "تعليم عالي"},
                    new EducationalLevel{Title = "دراسات عليا"},
                    new EducationalLevel{Title = "دكتوراه"},
                    new EducationalLevel{Title = "تدريب مهني"},
                    new EducationalLevel{Title = "تعلم عن بعد"},
                    new EducationalLevel{Title = "تعلم ذاتي"},
                };
                uow.DbContext.EducationalLevels.AddRange(EducationalLevels);
                uow.SaveChanges();
            }
            #endregion
        }
    }
}
