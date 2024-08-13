using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Reporting.Map.WebForms;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.NETCore;
using Moia.DoL.Enums;
using Moia.Shared.Helpers;
using Moia.Shared.Models;
using Moia.Shared.ViewModels;
using Moia.Shared.ViewModels.DTOs;
using Newtonsoft.Json;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using QRCoder;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text;

namespace Moia.BL.Repositories
{
    public interface IMuslimeRepository : IRepository<Muslime>
    {
        Task<MuslimeDto> getData(string OrderCode);
        Task<UserDataForVieweing> getDataForQuery(string Id);
        Task<PersonalDataDto> getPersonalData(int Muslime);
        Task<PersonalInformationDto> getPersonalInformation(int MuslimeId);
        Task<ContactAndInfoDataViewModel> getContactData(int MuslimeId);
        Task<FamilyAndWorkDto> getFamilyAndWork(int MuslimeId);
        Task<List<IsslamRecognitionData>> getIslamRecognitionWay(int MuslimeId);
        Task<AttachmentDto> getattachmentDto(int MuslimeId);
        Task<int> CreatePersonalData(PersonalDataDto model);
        Task<int> CreatePersonalInformation(PersonalInformationDto model);
        Task<int> CreateContactAndInfoData(ContactAndInfoDataViewModel model);
        Task<int> CreateFamilyAndWorkDto(FamilyAndWorkDto model);
        Task<int> CreateIslamRecognitionWays(IslamRecognitionWayDto model);
        Task<CustomeResponse> InsertAttachment(AttachmentDto model);
        void CreateOrder(int Id, OrderStatus orderStatus, OrderStage orderStage, string Description);
        Task<CustomeResponse> ChangeOrderState(string OrderCode, OrderStatus orderStatus, OrderStage orderStage, string Description);
        ViewerPagination<OrderListDto> getOrders(int page, int pageSize, string searchTerm, int? committeeId, int? DepartmentId, List<OrderStatus?> orderStatus);
        ViewerPagination<OrderListDto> getOrdersForPreview(int page, int pageSize, string searchTerm, int? committeeId, int? _DepartmentId, List<OrderStatus?> orderStatus);
        ViewerPagination<DepartmentDto> getAllDepartment(int page, int pageSize, string searchTerm);
        ViewerPagination<BranshListDto> GetAllBranchs(int page, int pageSize, string searchTerm);
        ViewerPagination<ResidenceIssuePlace> GetResidencePalcePaginated(int page, int pageSize, string searchTerm);
        ViewerPagination<Preacher> GetPreshersPaginated(int page, int pageSize, string searchTerm);
        ViewerPagination<OrderListDto> getFinishedOrders(int page, int pageSize, string searchTerm, int? committeeId, int? _DepartmentId);
        PrintCardView PrintCard(string Code);
        Task<bool> UpdateTawakkalnaCard(string Code);
        Task<bool> AddTawakkalnaCard(string Code);
        Task<bool> DeleteTawakkalnaCard(string Code);

    }

    public class MuslimeRepository : Repository<Muslime>, IMuslimeRepository
    {
        private readonly IUnitOfWork uow;

        public MuslimeRepository(IUnitOfWork _uow) : base(_uow)
        {
            uow = _uow;
        }
        public async Task<int> CreatePersonalData(PersonalDataDto model)
        {
            try
            {
                Muslime muslime = await uow.Muslime.DbSet.
                    Include(x => x.PersonalData).ThenInclude(x => x.Witness).
                    Include(x => x.PersonalData).ThenInclude(x => x.PreacherName)
                    .FirstOrDefaultAsync(x => x.ID == model.MuslimeId);
                if (muslime == null)
                {

                    //IslamDateHijry = model.IslamDateHijry.ToLocalTime(),
                    List<int> witnessIDs = new List<int>() { model.FirstWitness.ID, model.SecondWitness.ID };
                    var Witnesslist = uow.Witness.Where(x => witnessIDs.Contains(x.ID)).ToList();
                    PersonalData PersonalData = new PersonalData()
                    {
                        NameBeforeFristAr = model.NameBeforeFristAr,
                        NameBeforeMiddleAr = model.NameBeforeMiddleAr,
                        NameBeforeLastAr = model.NameBeforeLastAr,
                        NameAfter = model.NameAfter,
                        NameAfterEn = model.NameAfterEn,
                        NameBeforeFristEn = model.NameBeforeFristEn,
                        NameBeforeLastEn = model.NameBeforeLastEn,
                        NameBeforeMiddleEn = model.NameBeforeMiddleEn,
                        //IslamDate = DateTime.ParseExact(model.IslamDate, "dd-MM-yy", CultureInfo.InvariantCulture),
                        IslamDate = model.IslamDate.ToLocalTime(),
                        Witness = Witnesslist,
                    };
                    muslime = new Muslime()
                    {
                        PersonalData = PersonalData
                    };

                    uow.DbContext.Muslimes.Add(muslime);
                    uow.SaveChanges();
                    if (model.PreacherName != null)
                    {
                        muslime.PersonalData.PreacherName = model.PreacherName;
                        uow.SaveChanges();
                    }
                    var userid = uow.SessionServices.UserId != null ? uow.SessionServices.UserId : 0;
                    var userName = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == int.Parse(userid.ToString()))?.Name;
                    var userRole = uow.DbContext.MainUserRole.Include(x => x.Committee).FirstOrDefault(x => x.UserId == userid);
                    var message = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "CreateOrderMessage")?.ValueAr;
                    message = message.Replace("{DataEntry}", userRole.Committee.Title);
                    message += " "+userName;
                    CreateOrder(muslime.ID, OrderStatus.Create, OrderStage.DataEntry, message);
                    return muslime.ID;
                }
                else
                {
                    if (muslime.PersonalData != null)
                    {
                        List<int> witnessIDs = new List<int>() { model.FirstWitness.ID, model.SecondWitness.ID };
                        var Witnesslist = uow.Witness.Where(x => witnessIDs.Contains(x.ID)).ToList();
                        muslime.PersonalData.NameBeforeFristAr = model.NameBeforeFristAr;
                        muslime.PersonalData.NameBeforeMiddleAr = model.NameBeforeMiddleAr;
                        muslime.PersonalData.NameBeforeLastAr = model.NameBeforeLastAr;

                        muslime.PersonalData.NameBeforeFristEn = model.NameBeforeFristEn;
                        muslime.PersonalData.NameBeforeMiddleEn = model.NameBeforeMiddleEn;
                        muslime.PersonalData.NameBeforeLastEn = model.NameBeforeLastEn;

                        muslime.PersonalData.NameAfter = model.NameAfter;
                        muslime.PersonalData.NameAfterEn = model.NameAfterEn;
                        muslime.PersonalData.IslamDate = model.IslamDate.ToLocalTime();
                        //muslime.PersonalData.IslamDateHijry = model.IslamDateHijry.ToLocalTime();
                        if (model.PreacherName != null)
                        {
                            muslime.PersonalData.PreacherName = uow.DbContext.Preachers.FirstOrDefault(x => x.ID == model.PreacherName.ID);
                        }

                        if (muslime.PersonalData.Witness != null) { muslime.PersonalData.Witness.Clear(); muslime.PersonalData.Witness = Witnesslist; }
                        else muslime.PersonalData.Witness = Witnesslist;
                        uow.SaveChanges();
                        return muslime.ID;
                    }
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
                return 0;

            }
            return 0;
        }

        public async Task<int> CreatePersonalInformation(PersonalInformationDto model)
        {
            try
            {
                Muslime muslime = uow.Muslime.DbSet.Include(x => x.PersonalInformation).ThenInclude(x => x.ResidenceIssuePlace)
                   .FirstOrDefault(x => x.ID == model.MuslimeId);

                if (muslime != null)
                {
                    if (muslime.PersonalInformation == null)
                    {
                        PersonalInformation PersonalData = new PersonalInformation
                        {
                            DateOfBirth = model.DateOfBirth.ToLocalTime(),
                            DateOfEntryKingdom = model.DateOfEntryKingdom.ToLocalTime(),
                            PlaceOfBirth = model.PlaceOfBirth,
                            Nationality = await uow.DbContext.Countries.AsNoTracking().FirstOrDefaultAsync(x => x.ID == model.Nationality.ID),
                            Gender = model.Gender,
                            PreviousReligion = await uow.DbContext.Religions.AsNoTracking().FirstOrDefaultAsync(x => x.ID == model.PreviousReligion.ID),
                            PositionInFamily = model.PositionInFamily,
                            MaritalStatus = model.MaritalStatus != null ? model.MaritalStatus : 0,
                            HusbandName = model.HusbandName,
                            EducationalLevel = await uow.DbContext.EducationalLevels.FirstOrDefaultAsync(x => x.ID == model.EducationalLevel.ID),
                            ResidenceNumber = model.ResidenceNumber,
                            ResidenceIssueDate = model.ResidenceIssueDate.ToLocalTime(),
                            ResidenceIssuePlace = model.ResidenceIssuePlace != null ? uow.DbContext.ResidenceIssuePlace.FirstOrDefault(x => x.ID == model.ResidenceIssuePlace.ID) : null,
                            PassportNumber = model.PassportNumber,
                            DateOfPassportIssue = model.DateOfPassportIssue.ToLocalTime(),
                            PlaceOfPassportIssue = model.PlaceOfPassportIssue
                        };

                        muslime.PersonalInformation = PersonalData;
                        uow.SaveChanges();

                        return muslime.ID;
                    }
                    else
                    {
                        muslime.PersonalInformation.DateOfEntryKingdom = model.DateOfEntryKingdom.ToLocalTime();
                        muslime.PersonalInformation.DateOfBirth = model.DateOfBirth.ToLocalTime();
                        muslime.PersonalInformation.PlaceOfBirth = model.PlaceOfBirth;
                        muslime.PersonalInformation.Nationality = await uow.DbContext.Countries.AsNoTracking().FirstOrDefaultAsync(x => x.ID == model.Nationality.ID);
                        muslime.PersonalInformation.Gender = model.Gender;
                        muslime.PersonalInformation.PreviousReligion = await uow.DbContext.Religions.AsNoTracking().FirstOrDefaultAsync(x => x.ID == model.PreviousReligion.ID);
                        muslime.PersonalInformation.PositionInFamily = model.PositionInFamily;
                        muslime.PersonalInformation.MaritalStatus = model.MaritalStatus;
                        muslime.PersonalInformation.HusbandName = model.HusbandName;
                        muslime.PersonalInformation.EducationalLevel = await uow.DbContext.EducationalLevels.FirstOrDefaultAsync(x => x.ID == model.EducationalLevel.ID);
                        muslime.PersonalInformation.ResidenceNumber = model.ResidenceNumber;
                        muslime.PersonalInformation.ResidenceIssueDate = model.ResidenceIssueDate.ToLocalTime();
                        muslime.PersonalInformation.ResidenceIssuePlace = model.ResidenceIssuePlace != null ? uow.DbContext.ResidenceIssuePlace.FirstOrDefault(x => x.ID == model.ResidenceIssuePlace.ID) : null;
                        muslime.PersonalInformation.PassportNumber = model.PassportNumber;
                        muslime.PersonalInformation.DateOfPassportIssue = model.DateOfPassportIssue.ToLocalTime();
                        muslime.PersonalInformation.PlaceOfPassportIssue = model.PlaceOfPassportIssue;
                        uow.SaveChanges();
                        return muslime.ID;
                    }
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
                return 0;
            }
            return 0;
        }

        public async Task<int> CreateContactAndInfoData(ContactAndInfoDataViewModel model)
        {
            try
            {
                Muslime muslime = await uow.DbContext.Muslimes.FirstOrDefaultAsync(x => x.ID == model.MuslimeId);

                if (muslime != null)
                {
                    if (muslime.OriginalCountry == null)
                    {
                        if (
                            model.OriginalCountry.Country != null ||
                            model.OriginalCountry.City != null ||
                            model.OriginalCountry.Street != null ||
                            model.OriginalCountry.Region != null ||
                            model.OriginalCountry.DoorNumber != null
                            )
                        {
                            var country = await uow.DbContext.Countries.AsNoTracking().FirstOrDefaultAsync(x => x.ID == model.OriginalCountry.Country.ID);
                            OriginalCountry OriginalCountry = new OriginalCountry
                            {
                                Country = country,
                                City = model.OriginalCountry.City,
                                DoorNumber = model.OriginalCountry.DoorNumber,
                                Region = model.OriginalCountry.Region,
                                Street = model.OriginalCountry.Street
                            };
                            muslime.OriginalCountry = OriginalCountry;
                        }
                    }
                    else
                    {
                        Country country = null;
                        if (model.OriginalCountry.Country != null)
                            country = await uow.DbContext.Countries.AsNoTracking().FirstOrDefaultAsync(x => x.ID == model.OriginalCountry.Country.ID);
                        muslime.OriginalCountry.Country = country;
                        muslime.OriginalCountry.City = model.OriginalCountry.City;
                        muslime.OriginalCountry.DoorNumber = model.OriginalCountry.DoorNumber;
                        muslime.OriginalCountry.Region = model.OriginalCountry.Region;
                        muslime.OriginalCountry.Street = model.OriginalCountry.Street;
                    }

                    if (muslime.CurrentResidence == null)
                    {
                        if (
                            model.CurrentResidence.City != null ||
                            model.CurrentResidence.Region != null ||
                            model.CurrentResidence.Street != null ||
                            model.CurrentResidence.DoorNumber != null ||
                            model.CurrentResidence.EmergencyNumber != null
                            )
                        {
                            CurrentResidence CurrentResidence = new CurrentResidence
                            {
                                City = model.CurrentResidence.City,
                                Region = model.CurrentResidence.Region,
                                Street = model.CurrentResidence.Street,
                                DoorNumber = model.CurrentResidence.DoorNumber,
                                EmergencyNumber = model.CurrentResidence.EmergencyNumber
                            };
                            muslime.CurrentResidence = CurrentResidence;
                        }
                    }
                    else
                    {
                        muslime.CurrentResidence.City = model.CurrentResidence.City;
                        muslime.CurrentResidence.Region = model.CurrentResidence.Region;
                        muslime.CurrentResidence.Street = model.CurrentResidence.Street;
                        muslime.CurrentResidence.DoorNumber = model.CurrentResidence.DoorNumber;
                        muslime.CurrentResidence.EmergencyNumber = model.CurrentResidence.EmergencyNumber;
                    }

                    if (muslime.ContactData == null)
                    {
                        if (
                            model.ContactData.PhoneNumber != null ||
                            model.ContactData.HomeNumber != null ||
                            model.ContactData.Email != null ||
                            model.ContactData.WorkNumber != null
                            )
                        {
                            ContactData ContactData = new ContactData
                            {
                                Email = model.ContactData.Email,
                                HomeNumber = model.ContactData.HomeNumber,
                                PhoneNumber = model.ContactData.PhoneNumber,
                                WorkNumber = model.ContactData.WorkNumber,
                            };
                            muslime.ContactData = ContactData;
                        }
                    }
                    else
                    {
                        muslime.ContactData.Email = model.ContactData.Email;
                        muslime.ContactData.HomeNumber = model.ContactData.HomeNumber;
                        muslime.ContactData.PhoneNumber = model.ContactData.PhoneNumber;
                        muslime.ContactData.WorkNumber = model.ContactData.WorkNumber;
                    }

                    uow.SaveChanges();
                    return muslime.ID;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
                return 0;
            }
            return 0;
        }

        public async Task<int> CreateFamilyAndWorkDto(FamilyAndWorkDto model)
        {
            try
            {
                Muslime muslime = await uow.DbContext.Muslimes.FirstOrDefaultAsync(x => x.ID == model.MuslimeId);

                if (muslime != null)
                {

                    if (muslime.FamilyInformation == null)
                    {
                        if (
                            model.FamilyInformation.BoysNumber != null ||
                            model.FamilyInformation.GirlsNumber != null ||
                            model.FamilyInformation.MembersNumber != null
                            )
                        {
                            FamilyInformation FamilyInformation = new FamilyInformation
                            {
                                BoysNumber = model.FamilyInformation.BoysNumber,
                                MembersNumber = model.FamilyInformation.MembersNumber,
                                GirlsNumber = model.FamilyInformation.GirlsNumber
                            };
                            muslime.FamilyInformation = FamilyInformation;
                        }
                    }
                    else
                    {
                        muslime.FamilyInformation.BoysNumber = model.FamilyInformation.BoysNumber;
                        muslime.FamilyInformation.MembersNumber = model.FamilyInformation.MembersNumber;
                        muslime.FamilyInformation.GirlsNumber = model.FamilyInformation.GirlsNumber;
                    }

                    if (muslime.Work == null)
                    {
                        if (
                            model.Work.CompanyTitle != null ||
                            model.Work.Profession != null ||
                            model.Work.DirectManager != null ||
                            model.Work.City != null ||
                            model.Work.Street != null ||
                            model.Work.PostalBox != null ||
                            model.Work.Address != null ||
                            model.Work.PostalCode != null
                            )
                        {
                            Work Work = new Work
                            {
                                CompanyTitle = model.Work.CompanyTitle,
                                Profession = model.Work.Profession,
                                DirectManager = model.Work.DirectManager,
                                City = model.Work.City,
                                Street = model.Work.Street,
                                Address = model.Work.Address,
                                PostalBox = model.Work.PostalBox,
                                PostalCode = model.Work.PostalCode,
                            };
                            muslime.Work = Work;
                        }
                    }
                    else
                    {
                        muslime.Work.CompanyTitle = model.Work.CompanyTitle;
                        muslime.Work.Profession = model.Work.Profession;
                        muslime.Work.DirectManager = model.Work.DirectManager;
                        muslime.Work.City = model.Work.City;
                        muslime.Work.Street = model.Work.Street;
                        muslime.Work.Address = model.Work.Address;
                        muslime.Work.PostalBox = model.Work.PostalBox;
                        muslime.Work.PostalCode = model.Work.PostalCode;
                    }

                    uow.SaveChanges();
                    return muslime.ID;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
                return 0;
            }
            return 0;
        }

        public async Task<int> CreateIslamRecognitionWays(IslamRecognitionWayDto model)
        {
            try
            {
                Muslime muslime = await uow.DbContext.Muslimes.Include(x => x.IsslamRecognition).FirstOrDefaultAsync(x => x.ID == model.MuslimeId);

                if (muslime != null)
                {
                    var _eee = model.IslamRecognitionWay.Select(x => new IsslamRecognition
                    {
                        ID = x.ID,
                        Title = x.Title
                    }).ToList();
                    foreach (var item in _eee)
                    {
                        if (!muslime.IsslamRecognition.Select(x => x.ID).Contains(item.ID))
                        {
                            muslime.IsslamRecognition.Add(item);
                        }
                    }
                    uow.SaveChanges();
                    return muslime.ID;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
                return 0;
            }
            return 0;
        }

        public async Task<CustomeResponse> InsertAttachment(AttachmentDto model)
        {
            try
            {
                Muslime muslime = await uow.DbContext.Muslimes.Include(x => x.Attachment).FirstOrDefaultAsync(x => x.ID == model.MuslimeId);

                if (muslime != null)
                {
                    #region Insert Or Update Attachments
                    if (muslime.Attachment == null)
                    {
                        if (model.Personal != null && model.Passport != null && model.Accomodation != null)
                        {
                            List<Attachment> Attachments = new List<Attachment>() {
                    new Attachment{
                        ImageType = ImageType.Personal,
                        AttachmentValue =await uow.ConvertIFormFileToByteArray(model.Personal , 250,250) },

                        new Attachment{
                        ImageType = ImageType.Accommodation,
                        AttachmentValue =await uow.ConvertIFormFileToByteArray(model.Accomodation, 250,250) },

                        new Attachment{
                        ImageType = ImageType.Passport,
                        AttachmentValue =await uow.ConvertIFormFileToByteArray(model.Passport, 250,250) },
                    };
                            muslime.Attachment = Attachments;
                            uow.SaveChanges();
                        }
                    }
                    else
                    {
                        if (model.Accomodation != null)
                        {
                            var oldvalue = muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Accommodation);
                            muslime.Attachment.Remove(oldvalue);

                            muslime.Attachment.Add(new Attachment
                            {
                                ImageType = ImageType.Accommodation,
                                AttachmentValue = await uow.ConvertIFormFileToByteArray(model.Accomodation, 250, 250)
                            });
                        }

                        if (model.Passport != null)
                        {
                            var oldvalue = muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Passport);
                            muslime.Attachment.Remove(oldvalue);

                            muslime.Attachment.Add(new Attachment
                            {
                                ImageType = ImageType.Passport,
                                AttachmentValue = await uow.ConvertIFormFileToByteArray(model.Passport, 250, 250)
                            });
                        }

                        if (model.Personal != null)
                        {
                            var oldvalue = muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Personal);
                            muslime.Attachment.Remove(oldvalue);

                            muslime.Attachment.Add(new Attachment
                            {
                                ImageType = ImageType.Personal,
                                AttachmentValue = await uow.ConvertIFormFileToByteArray(model.Personal, 250, 250)
                            });
                        }

                        uow.SaveChanges();
                    }
                    #endregion
                    CustomeResponse response = new CustomeResponse();
                    response.Code = uow.DbContext.Orders.Include(x => x.Muslime).FirstOrDefault(x => x.Muslime.ID == muslime.ID)?.Code;
                    return response;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
            }
            return null;
        }


        public void CreateOrder(int MuslimeId, OrderStatus orderStatus, OrderStage orderStage, string Description)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                HijriCalendar hijriCalendar = new HijriCalendar();
                string hijriYear = hijriCalendar.GetYear(currentDate).ToString();

                var UserId = uow.SessionServices.UserId;
                var order = uow.DbContext.Orders.Include(x => x.Muslime).FirstOrDefault(x => x.Muslime.ID == MuslimeId);
                if (order == null)
                {
                    Muslime muslime = uow.Muslime.DbSet.FirstOrDefault(x => x.ID == MuslimeId);

                    var committeeId = uow.DbContext.MainUserRole.Include(x => x.Role).FirstOrDefault(x => x.UserId == UserId &&
                                                                        x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.DataEntry.ToString()).ID).CommitteeId;
                    var committee = uow.DbContext.Committees.Include(x => x.Department).ThenInclude(x => x.MinistryBransh).FirstOrDefault(x => x.ID == committeeId);

                    uow.DbContext.Orders.Add(new Order
                    {
                        Code = hijriYear + "-" + committee.Department.MinistryBransh.Code + "-" + (uow.DbContext.Orders.Count() + 1).ToString(),
                        Stage = orderStage,
                        Muslime = muslime,
                        CreationDate = DateTime.Now,
                        Committee = committee,
                        DataEntry = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == UserId),
                        OrderHistories = new List<OrderHistory> { new OrderHistory {
                            Description = Description,
                            ActionDate = DateTime.UtcNow,
                            OrderStatus = orderStatus,
                            UserId = uow.SessionServices.UserId,
                            UserName =uow.SessionServices.UserId!=null ? uow.User.FirstOrDefault(x => x.ID == int.Parse(uow.SessionServices.UserId.ToString())).Name : "" ,
                        } }
                    });
                    uow.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
            }
        }
        public async Task<CustomeResponse> ChangeOrderState(string OrderCode, OrderStatus orderStatus, OrderStage orderStage, string Description)
        {
            try
            {
                CustomeResponse response = new CustomeResponse();
                var UserId = uow.SessionServices.UserId;
                var Order = await uow.DbContext.Orders.
                                               Include(x => x.DataEntry).
                                               Include(x => x.Muslime).
                                                    ThenInclude(x => x.PersonalData).
                                               Include(x => x.Muslime).
                                                    ThenInclude(x => x.PersonalInformation).
                                               Include(x => x.OrderHistories).
                                               Include(x => x.Committee).
                                               ThenInclude(x => x.Department).
                                               FirstOrDefaultAsync(x => x.Code == OrderCode);

                #region Identify LoggedInUser
                var user = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == UserId)?.Name;
                var userRole = uow.DbContext.MainUserRole.
                                                Include(x => x.Role).
                                                Include(x => x.User).
                                                Include(x => x.Committee).
                                                    ThenInclude(x => x.Department).
                                                Include(x => x.Department).
                                                ThenInclude(x => x.MinistryBransh).
                                                Include(x => x.Bransh).
                                                FirstOrDefault(x => x.UserId == UserId);

                string description = "";
                switch (orderStatus)
                {
                    case OrderStatus.Send:
                        switch (orderStage)
                        {
                            case OrderStage.Committee:
                                var messagefromDataEntry = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "SendOrderToCommitteeDescription")?.ValueAr;
                                if (messagefromDataEntry == null) messagefromDataEntry = "";
                                messagefromDataEntry += $" ( {userRole.Committee?.Title} ) \n بواسطة :  {userRole.User.Name}";
                                description = messagefromDataEntry;
                                break;
                            case OrderStage.Department:
                                var messagefromCommitteeManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "SendOrderToDepartmentDescription")?.ValueAr;
                                if (messagefromCommitteeManager == null) messagefromCommitteeManager = "";
                                messagefromCommitteeManager += $" ( {userRole.Committee.Department.Title} ) بواسطة :  {userRole.User.Name}";
                                description = messagefromCommitteeManager;
                                break;
                            case OrderStage.Branch:
                                var messagefromDeptManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "SendOrderToBranchDescription")?.ValueAr;
                                if (messagefromDeptManager == null) messagefromDeptManager = "";
                                messagefromDeptManager += $" ( {userRole.Department.MinistryBransh.Title} ) بواسطة :  {userRole.User.Name}";
                                description = messagefromDeptManager;
                                break;
                            case OrderStage.ReadyToPrintCard:
                                var messagefromBranshManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "ConfirmBranchManagerDescription")?.ValueAr;
                                if (messagefromBranshManager == null) messagefromBranshManager = "";
                                messagefromBranshManager += $" ( {userRole.Bransh.Title} ) بواسطة :  {userRole.User.Name}";
                                description = messagefromBranshManager;
                                break;
                            default:
                                break;
                        }
                        break;
                    case OrderStatus.Accept:
                        switch (orderStage)
                        {
                            case OrderStage.Department:
                                var messagefromCommitteeManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "SendOrderToDepartmentDescription")?.ValueAr;
                                if (messagefromCommitteeManager == null) messagefromCommitteeManager = "";
                                messagefromCommitteeManager += $" ( {userRole.Committee.Department.Title} ) بواسطة :  {userRole.User.Name}";
                                description = messagefromCommitteeManager;
                                break;
                            case OrderStage.Branch:
                                var messagefromDeptManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "SendOrderToBranchDescription")?.ValueAr;
                                if (messagefromDeptManager == null) messagefromDeptManager = "";
                                messagefromDeptManager += $" ( {userRole.Department.MinistryBransh.Title} ) بواسطة :  {userRole.User.Name}";
                                description = messagefromDeptManager;
                                break;
                            case OrderStage.ReadyToPrintCard:
                                var messagefromBranshManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "ConfirmBranchManagerDescription")?.ValueAr;
                                if (messagefromBranshManager == null) messagefromBranshManager = "";
                                messagefromBranshManager += $" ( {userRole.Bransh.Title} ) بواسطة :  {userRole.User.Name}";
                                description = messagefromBranshManager;
                                break;
                            default:
                                break;
                        }
                        break;
                    case OrderStatus.Reject:
                        switch (userRole.Role.Code)
                        {
                            case RoleCodes.CommitteeManager:
                                var messagefromCommitteeManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "RefuseCommitteeManagerMessage")?.ValueAr;
                                if (messagefromCommitteeManager == null) messagefromCommitteeManager = "";
                                messagefromCommitteeManager = messagefromCommitteeManager.Replace("{CommitteeManager}", userRole.User.Name);
                                messagefromCommitteeManager += $"{Description}";
                                description = messagefromCommitteeManager;
                                break;
                            case RoleCodes.DepartmentManager: 
                                var messagefromDeptManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "RefuseDepartmentManagerMessage")?.ValueAr;
                                if (messagefromDeptManager == null) messagefromDeptManager = "";
                                messagefromDeptManager = messagefromDeptManager.Replace("{DepartmentManager}", userRole.User.Name);
                                messagefromDeptManager += $"{Description}";
                                description = messagefromDeptManager;
                                break;
                            case RoleCodes.BranchManager:
                                var messagefromBranchManager = uow.DbContext.Localizations.FirstOrDefault(x => x.Key == "RefuseBranshManagerMessage")?.ValueAr;
                                if (messagefromBranchManager == null) messagefromBranchManager = "";
                                messagefromBranchManager = messagefromBranchManager.Replace("{BranshManager}", userRole.User.Name);
                                messagefromBranchManager += $"{Description}";
                                description = messagefromBranchManager;
                                break;
                            default:
                                break;
                        }
                        break;
                    case OrderStatus.Finished:
                        break;
                    case OrderStatus.Printed:
                        break;
                    default:
                        break;
                }
                #endregion




                if (Order != null)
                {
                    Order.Stage = orderStage;
                    if (Order.OrderHistories.Count == 0) Order.OrderHistories = new List<OrderHistory>();
                    Order.OrderHistories.Add(new OrderHistory
                    {
                        Description = description,
                        ActionDate = DateTime.UtcNow,
                        OrderStatus = orderStatus,
                        UserId = uow.SessionServices.UserId,
                        UserName = UserId != null ? uow.User.FirstOrDefault(x => x.ID == int.Parse(UserId.ToString())).Name : "",
                    });
                    uow.SaveChanges();
                    response.Code = Order.Muslime.ID.ToString();

                    // ********** Add Card Into Tawakkalna Application *******************
                    if (orderStage == OrderStage.ReadyToPrintCard && orderStatus != OrderStatus.Printed)
                    {
                        try
                        {
                            await AddTawakkalnaCard(Order.Code);
                        }
                        catch (Exception ex)
                        {
                            uow.DbContext.Exceptions.Add(new Exceptions
                            {
                                Message = ex.Message,
                                Stacktrace = ex.StackTrace
                            });
                            uow.SaveChanges();
                        }

                    }

                    switch (orderStage)
                    {
                        case OrderStage.DataEntry:

                            //Send To Committee Data Entry For Order Rejection
                            if (orderStatus == OrderStatus.Reject)
                            {
                                string UserEmail = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == Order.DataEntry.ID)?.Email;
                                if (UserEmail != null)
                                {
                                    await Task.Run(() =>
                                    {
                                        uow.MailServices.SendNotificationEmail(UserEmail, "EmailSubject",
                                            Description, true, null, null, Path.Combine(uow.ContentRootPath, "wwwroot")
                                            );
                                    });
                                }
                            }
                            break;
                        case OrderStage.ReadyToPrintCard:
                            //Send To Committee Data Entry For Order Rejection
                            if (orderStatus == OrderStatus.Accept)
                            {
                                string UserEmail = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == Order.DataEntry.ID)?.Email;
                                if (UserEmail != null)
                                {
                                    await Task.Run(() =>
                                    {
                                        uow.MailServices.SendNotificationEmail(UserEmail, "EmailSubject",
                                            Description, true, null, null, Path.Combine(uow.ContentRootPath, "wwwroot")
                                            );
                                    });
                                }
                            }
                            break;
                        case OrderStage.Committee:

                            //Send To Committee Manager 
                            if (orderStatus == OrderStatus.Send)
                            {
                                var CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == Order.DataEntry.ID)?.CommitteeId;
                                if (CommitteeId != null)
                                {
                                    var CommitteeManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.CommitteeId == CommitteeId &&
                                                            x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.CommitteeManager.ToString()).ID)?.UserId;

                                    if (CommitteeManagerId != null)
                                    {
                                        var CommitteeManagerEmail = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == CommitteeManagerId).Email;
                                        if (CommitteeManagerEmail != null)
                                        {
                                            await Task.Run(() =>
                                            {
                                                uow.MailServices.SendNotificationEmail(CommitteeManagerEmail, "EmailSubject",
                                                    Description, true, null, null, Path.Combine(uow.ContentRootPath, "wwwroot")
                                                    );
                                            });
                                        }
                                    }
                                }
                            }
                            break;
                        case OrderStage.Department:

                            //Send To Department Manager 
                            if (orderStatus == OrderStatus.Accept)
                            {
                                var _CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == Order.DataEntry.ID)?.CommitteeId;
                                var DepartmentId = uow.DbContext.Committees.Include(x => x.Department).FirstOrDefault(x => x.ID == _CommitteeId)?.DepartmentId;
                                if (DepartmentId != null)
                                {
                                    var DepartmentManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.DepartmentId == DepartmentId &&
                                                            x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.DepartmentManager.ToString()).ID)?.UserId;

                                    if (DepartmentManagerId != null)
                                    {
                                        var DepartmentManagerEmail = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == DepartmentManagerId).Email;
                                        if (DepartmentManagerEmail != null)
                                        {
                                            await Task.Run(() =>
                                            {
                                                uow.MailServices.SendNotificationEmail(DepartmentManagerEmail, "EmailSubject",
                                                    Description, true, null, null, Path.Combine(uow.ContentRootPath, "wwwroot")
                                                    );
                                            });
                                        }
                                    }
                                }
                            }

                            break;
                        case OrderStage.Branch:
                            //Send To Branch Manager 
                            if (orderStatus == OrderStatus.Accept)
                            {
                                var _CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == Order.DataEntry.ID)?.CommitteeId;
                                var BranshId = uow.DbContext.Committees.Include(x => x.Department).ThenInclude(x => x.MinistryBransh).FirstOrDefault(x => x.ID == _CommitteeId)?.Department.BranshID;
                                if (BranshId != null)
                                {
                                    var BranshManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.BranshId == BranshId &&
                                                            x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString()).ID)?.UserId;

                                    if (BranshManagerId != null)
                                    {
                                        var BranshManagerEmail = uow.DbContext.MainUsers.FirstOrDefault(x => x.ID == BranshManagerId).Email;
                                        if (BranshManagerEmail != null)
                                        {
                                            await Task.Run(() =>
                                            {
                                                uow.MailServices.SendNotificationEmail(BranshManagerEmail, "EmailSubject",
                                                    Description, true, null, null, Path.Combine(uow.ContentRootPath, "wwwroot")
                                                    );
                                            });
                                        }
                                    }
                                }

                            }
                            break;
                        default:
                            break;
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace,
                });
                uow.SaveChanges();
            }
            return null;
        }

        public async Task<bool> UpdateTawakkalnaCard(string Code)
        {
            //try
            //{
            //    var Order = uow.DbContext.Orders.
            //        Include(x => x.Committee).
            //        ThenInclude(x => x.Department).
            //        Include(x => x.Muslime).
            //        FirstOrDefault(x => x.Code == Code);
            //    if (Order != null)
            //    {
            //        var Toaken = uow.IntegrationRepository.GetToakenAsync("en");

            //        var BranshID = Order.Committee.Department.BranshID;
            //        var Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString());
            //        var ManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.BranshId == BranshID && x.RoleId == Role.ID)?.UserId;
            //        var Signature = uow.DbContext.MainUsers.
            //                                    Include(x => x.Attachment).
            //                                    FirstOrDefault(x => x.ID == ManagerId)?.Attachment.AttachmentValue;

            //        var res = uow.IntegrationRepository.ManageCardAsync(new ManageCardDto
            //        {
            //            actionType = ActionType.UpdateCard,
            //            BearerToaken = Toaken.Result,
            //            referenceNo = Order.Code,
            //            documentNo = long.Parse(Order.Muslime.PersonalInformation.ResidenceNumber),
            //            uniqueCardId = long.Parse(uow.Configuration.GetSection("TawakkalnaIntegrationSettings:uniqueCardId").Value),
            //            language = "ar",
            //            cardAttributes = new List<cardAttributes>
            //                    {
            //                        new cardAttributes
            //                        {
            //                            key = "CA.NameBeforeAr",
            //                            value= Order.Muslime.PersonalData.NameBeforeFristAr + " " +
            //                                   Order.Muslime.PersonalData.NameBeforeMiddleAr + " " +
            //                                   Order.Muslime.PersonalData.NameBeforeLastAr
            //                        },new cardAttributes
            //                        {
            //                            key = "CA.NameBeforeEn",
            //                            value=Order.Muslime.PersonalData.NameBeforeFristEn + " " +
            //                                  Order.Muslime.PersonalData.NameBeforeMiddleEn + " " +
            //                                  Order.Muslime.PersonalData.NameBeforeLastEn
            //                        },new cardAttributes
            //                        {
            //                            key = "CA.NationalityAr",
            //                            value=Order.Muslime.PersonalInformation.Nationality.NameAr
            //                        },new cardAttributes
            //                        {
            //                            key = "CA.NationalityEn",
            //                            value=Order.Muslime.PersonalInformation.Nationality.NameEn
            //                        },new cardAttributes
            //                        {
            //                            key = "CA.PassportNumber",
            //                            value=Order.Muslime.PersonalInformation.PassportNumber
            //                        },new cardAttributes
            //                        {
            //                            key = "CA.NameAfter",
            //                            value=Order.Muslime.PersonalData.NameAfter
            //                        },new cardAttributes
            //                        {
            //                            key = "CA.NameAfterEn",
            //                            value=Order.Muslime.PersonalData.NameAfterEn
            //                        },new cardAttributes
            //                        {
            //                            key = "CA.IslamDate",
            //                            value=Order.Muslime.PersonalData.IslamDate.ToString("dd/MM/yyyy")
            //                        },
            //                    },
            //            backCardAttributes = new List<cardAttributes>
            //                    {
            //                        new cardAttributes
            //                        {
            //                            key = "Signature",
            //                            value=Convert.ToBase64String(Signature)
            //                        }
            //                    }
            //        });

            //        if (res != null) return true;

            //    }
            //}
            //catch (Exception ex)
            //{
            //    uow.DbContext.Exceptions.Add(new Exceptions
            //    {
            //        Message = ex.Message,
            //        Stacktrace = ex.StackTrace
            //    });
            //    uow.SaveChanges();
            //    return false;
            //}

            try
            {
                //var UserId = uow.SessionServices.UserId;
                var Order = await uow.DbContext.Orders.
                                               Include(x => x.DataEntry).
                                               Include(x => x.Muslime).
                                                    ThenInclude(x => x.PersonalData).
                                               Include(x => x.Muslime).
                                                    ThenInclude(x => x.PersonalInformation).
                                               Include(x => x.OrderHistories).
                                               Include(x => x.Committee).
                                               ThenInclude(x => x.Department).
                                               FirstOrDefaultAsync(x => x.Code == Code);

                if (Order.Muslime.PersonalInformation.ResidenceNumber == null)
                {
                    uow.DbContext.Exceptions.Add(new Exceptions
                    {
                        Message = "Order.Muslime.PersonalInformation.ResidenceNumber is null ",
                        Stacktrace = "Order.Muslime.PersonalInformation.ResidenceNumber is null  when update card into tawakklna"
                    });
                    uow.SaveChanges();
                    return false;
                }

                var Toaken = await uow.IntegrationRepository.GetToakenAsync("en");

                var BranshID = Order.Committee?.Department?.BranshID;
                var Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString());
                var ManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.BranshId == BranshID && x.RoleId == Role.ID)?.UserId;
                var Signature = uow.DbContext.MainUsers.
                                            Include(x => x.Attachment).
                                            FirstOrDefault(x => x.ID == ManagerId)?.Attachment.AttachmentValue;

                HijriCalendar hijriCalendar = new HijriCalendar();
                int hijriYear = hijriCalendar.GetYear(Order.Muslime.PersonalData.IslamDate);
                int hijriMonth = hijriCalendar.GetMonth(Order.Muslime.PersonalData.IslamDate);
                int hijriDay = hijriCalendar.GetDayOfMonth(Order.Muslime.PersonalData.IslamDate);

                string _value = EncryptHelper.Encrypt(Order.Muslime.ID.ToString());
                var request = uow._httpContextAccessor.HttpContext.Request;

                var res = uow.IntegrationRepository.ManageCardAsync(new ManageCardDto
                {
                    actionType = ActionType.UpdateCard,
                    BearerToaken = Toaken,
                    referenceNo = Order.Muslime.PersonalInformation.ResidenceNumber.ToString(),
                    documentNo = long.Parse(Order.Muslime.PersonalInformation.ResidenceNumber),
                    uniqueCardId = long.Parse(uow.Configuration.GetSection("TawakkalnaIntegrationSettings:uniqueCardId").Value),
                    language = "ar",
                    cardAttributes = new List<cardAttributes>
            {
                new cardAttributes
                {
                    key = "PassportNumber",
                    value=Order.Muslime.PersonalInformation.PassportNumber
                },
                new cardAttributes
                {
                    key = "NameAfter",
                    value=Order.Muslime.PersonalData.NameAfter
                },
                new cardAttributes
                {
                    key = "IslamDateH",
                    value= $"{hijriDay:00}/{hijriMonth:00}/{hijriYear:0000}"
                },
                new cardAttributes
                {
                    key = "NameAfterEn",
                    value=Order.Muslime.PersonalData.NameAfterEn
                },
                new cardAttributes
                {
                    key = "IslamDate",
                    value=Order.Muslime.PersonalData.IslamDate.ToString("dd/MM/yyyy")
                },
                //new cardAttributes
                //{
                //    key = "NameBeforeAr",
                //    value= Order.Muslime.PersonalData.NameBeforeFristAr + " " +
                //           Order.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                //           Order.Muslime.PersonalData.NameBeforeLastAr
                //},new cardAttributes
                //{
                //    key = "NameBeforeEn",
                //    value=Order.Muslime.PersonalData.NameBeforeFristEn + " " +
                //          Order.Muslime.PersonalData.NameBeforeMiddleEn + " " +
                //          Order.Muslime.PersonalData.NameBeforeLastEn
                //},new cardAttributes
                //{
                //    key = "NationalityAr",
                //    value=Order.Muslime.PersonalInformation.Nationality.NameAr
                //},new cardAttributes
                //{
                //    key = "NationalityEn",
                //    value=Order.Muslime.PersonalInformation.Nationality.NameEn
                //},
            },
                    backCardAttributes = new List<cardAttributes>
            {
                //new cardAttributes
                //{
                //    key = "Signature",
                //    value=Convert.ToBase64String(Signature)
                //},
                new cardAttributes
                {
                    key = "cardQrCode",
                    value= $"{request.Scheme}://{request.Host}{request.PathBase}/#/query-info/details/{_value.Replace("=", "_")}"
                }
            }
                });
                return res != null ? true : false;

            }

            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                });
                uow.SaveChanges();
            }

            return false;
        }

        public async Task<bool> AddTawakkalnaCard(string Code)
        {

            try
            {
                var Order = await uow.DbContext.Orders.
                                               Include(x => x.DataEntry).
                                               Include(x => x.Muslime).
                                                    ThenInclude(x => x.PersonalData).
                                               Include(x => x.Muslime).
                                                    ThenInclude(x => x.PersonalInformation).
                                               Include(x => x.OrderHistories).
                                               Include(x => x.Committee).
                                               ThenInclude(x => x.Department).
                                               FirstOrDefaultAsync(x => x.Code == Code);

                if (Order.Muslime.PersonalInformation.ResidenceNumber == null)
                {
                    uow.DbContext.Exceptions.Add(new Exceptions
                    {
                        Message = "Order.Muslime.PersonalInformation.ResidenceNumber is null ",
                        Stacktrace = "Order.Muslime.PersonalInformation.ResidenceNumber is null  when add card into tawakklna"
                    });
                    uow.SaveChanges();
                    return false;
                }
                var Toaken = await uow.IntegrationRepository.GetToakenAsync("en");

                var BranshID = Order.Committee?.Department?.BranshID;
                var Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString());
                var ManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.BranshId == BranshID && x.RoleId == Role.ID)?.UserId;
                var Signature = uow.DbContext.MainUsers.
                                            Include(x => x.Attachment).
                                            FirstOrDefault(x => x.ID == ManagerId)?.Attachment.AttachmentValue;

                HijriCalendar hijriCalendar = new HijriCalendar();
                int hijriYear = hijriCalendar.GetYear(Order.Muslime.PersonalData.IslamDate);
                int hijriMonth = hijriCalendar.GetMonth(Order.Muslime.PersonalData.IslamDate);
                int hijriDay = hijriCalendar.GetDayOfMonth(Order.Muslime.PersonalData.IslamDate);

                string _value = EncryptHelper.Encrypt(Order.Muslime.ID.ToString());
                var request = uow._httpContextAccessor.HttpContext.Request;

                var res = await uow.IntegrationRepository.ManageCardAsync(new ManageCardDto
                {
                    actionType = ActionType.AddCard,
                    BearerToaken = Toaken,
                    referenceNo = Order.Muslime.PersonalInformation.ResidenceNumber.ToString(),
                    documentNo = long.Parse(Order.Muslime.PersonalInformation.ResidenceNumber),
                    uniqueCardId = long.Parse(uow.Configuration.GetSection("TawakkalnaIntegrationSettings:uniqueCardId").Value),
                    language = "ar",
                    cardAttributes = new List<cardAttributes>
                        {
                            new cardAttributes
                            {
                                key = "PassportNumber",
                                value=Order.Muslime.PersonalInformation.PassportNumber
                            },
                            new cardAttributes
                            {
                                key = "NameAfter",
                                value=Order.Muslime.PersonalData.NameAfter
                            },
                            new cardAttributes
                            {
                                key = "IslamDateH",
                                value= $"{hijriDay:00}/{hijriMonth:00}/{hijriYear:0000}"
                            },
                            new cardAttributes
                            {
                                key = "NameAfterEn",
                                value=Order.Muslime.PersonalData.NameAfterEn
                            },
                            new cardAttributes
                            {
                                key = "IslamDate",
                                value=Order.Muslime.PersonalData.IslamDate.ToString("dd/MM/yyyy")
                            },
                            //new cardAttributes
                            //{
                            //    key = "NameBeforeAr",
                            //    value= Order.Muslime.PersonalData.NameBeforeFristAr + " " +
                            //           Order.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                            //           Order.Muslime.PersonalData.NameBeforeLastAr
                            //},new cardAttributes
                            //{
                            //    key = "NameBeforeEn",
                            //    value=Order.Muslime.PersonalData.NameBeforeFristEn + " " +
                            //          Order.Muslime.PersonalData.NameBeforeMiddleEn + " " +
                            //          Order.Muslime.PersonalData.NameBeforeLastEn
                            //},new cardAttributes
                            //{
                            //    key = "NationalityAr",
                            //    value=Order.Muslime.PersonalInformation.Nationality.NameAr
                            //},new cardAttributes
                            //{
                            //    key = "NationalityEn",
                            //    value=Order.Muslime.PersonalInformation.Nationality.NameEn
                            //},
                        },
                    backCardAttributes = new List<cardAttributes> {
                                                    //new cardAttributes
                                                    //{
                                                    //    key = "Signature",
                                                    //    value=Convert.ToBase64String(Signature)
                                                    //},
                                                    new cardAttributes
                                                    {
                                                        key = "cardQrCode",
                                                        value= $"{request.Scheme}://{request.Host}{request.PathBase}/#/query-info/details/{_value.Replace("=", "_")}"
                                                    }
                                                }
                });

                return res != null ? true : false;

            }

            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                });
                uow.SaveChanges();
            }

            return false;
        }

        public async Task<bool> DeleteTawakkalnaCard(string Code)
        {
            try
            {
                var Order = uow.DbContext.Orders.
                    Include(x => x.Committee).
                    ThenInclude(x => x.Department).
                    Include(x => x.Muslime).
                    ThenInclude(x => x.PersonalInformation).
                    FirstOrDefault(x => x.Code == Code);

                if (Order.Muslime.PersonalInformation.ResidenceNumber == null)
                {
                    uow.DbContext.Exceptions.Add(new Exceptions
                    {
                        Message = "Order.Muslime.PersonalInformation.ResidenceNumber is null ",
                        Stacktrace = "Order.Muslime.PersonalInformation.ResidenceNumber is null  when delete card into tawakklna"
                    });
                    uow.SaveChanges();
                    return false;
                }

                var Toaken = await uow.IntegrationRepository.GetToakenAsync("en");

                var BranshID = Order.Committee.Department.BranshID;
                var Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString());
                var ManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.BranshId == BranshID && x.RoleId == Role.ID)?.UserId;
                var Signature = uow.DbContext.MainUsers.
                                            Include(x => x.Attachment).
                                            FirstOrDefault(x => x.ID == ManagerId)?.Attachment.AttachmentValue;

                var res = await uow.IntegrationRepository.DeleteCardAsync(new ManageCardDto
                {
                    actionType = ActionType.DeleteCard,
                    BearerToaken = Toaken,
                    referenceNo = Order.Code,
                    documentNo = long.Parse(Order.Muslime.PersonalInformation.ResidenceNumber),
                    uniqueCardId = long.Parse(uow.Configuration.GetSection("TawakkalnaIntegrationSettings:uniqueCardId").Value),
                    language = "ar",

                });

                if (res != null) return true;


            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                });
                uow.SaveChanges();
                return false;
            }

            return false;
        }


        public ViewerPagination<OrderListDto> getOrders(int page, int pageSize, string searchTerm, int? committeeId, int? _DepartmentId, List<OrderStatus?> orderStatus)
        {
            IQueryable<Order> myData;
            ViewerPagination<OrderListDto> viewerPagination = new ViewerPagination<OrderListDto>();
            int myDataCount = 0;
            OrderStage orderStage = OrderStage.Other;
            var UserId = uow.SessionServices.UserId;
            var Role = uow.DbContext.MainUserRole.AsNoTracking().Include(x => x.Role).FirstOrDefault(x => x.UserId == UserId)?.Role;

            //if (Role?.Name == UserType.DataEntry.ToString()) orderStage = OrderStage.DataEntry;
            //else if (Role?.Name == UserType.CommitteeManager.ToString()) orderStage = OrderStage.Committee;
            //else if (Role?.Name == UserType.DepartmentManager.ToString() || Role?.Name == UserType.NegoiatedDepartmentManager.ToString()) orderStage = OrderStage.Department;
            //else if (Role?.Name == UserType.BranchManager.ToString() || Role?.Name == UserType.NegoiatedBranchManager.ToString()) orderStage = OrderStage.Branch;
            //else orderStage = OrderStage.Other;

            UserType userType = uow.SessionServices.UserType;

            switch (userType)
            {
                case UserType.DataEntry:
                    orderStage = OrderStage.DataEntry;
                    break;
                case UserType.CommitteeManager:
                    orderStage = OrderStage.Committee;
                    break;
                case UserType.DepartmentManager:
                    orderStage = OrderStage.Department;
                    break;
                case UserType.BranchManager:
                    orderStage = OrderStage.Branch;
                    break;
                case UserType.SuperAdmin:
                    orderStage = OrderStage.Other;
                    break;
                case UserType.NegoiatedDepartmentManager:
                    orderStage = OrderStage.Department;
                    break;
                case UserType.NegoiatedBranchManager:
                    orderStage = OrderStage.Branch;
                    break;
                case UserType.None:
                    break;
                default:
                    break;
            }

            switch (orderStage)
            {
                case OrderStage.DataEntry:
                    var _CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == UserId &&
                                                                                     x.RoleId == uow.DbContext.MainRoles.
                                                                                                FirstOrDefault(x => x.Code == UserType.DataEntry.ToString()).ID)?.
                                                                                                CommitteeId;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.DataEntry).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == _CommitteeId).
                                                    Orders.
                                                    Where(x => x.Stage == OrderStage.DataEntry).
                                                    Where(x => x.DataEntry.ID == UserId && (orderStatus.Contains(x.OrderHistories.OrderBy(h => h.ActionDate).LastOrDefault().OrderStatus) || orderStatus.Count() == 0)).
                                                    Where(x => x.Code.Contains(searchTerm) || searchTerm == null).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.DataEntry).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == _CommitteeId).
                                                    Orders.
                                                    Where(x => x.Stage == OrderStage.DataEntry).
                                                    Where(x => x.DataEntry.ID == UserId &&
                                                         (orderStatus.Contains(x.OrderHistories.OrderBy(h => h.ActionDate).LastOrDefault().OrderStatus) || orderStatus.Count() == 0)).
                                                    AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        MuslimeId = x.Muslime.ID,
                        Stage = "موظف الجمعية",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    break;
                case OrderStage.Committee:
                    #region Committee Orders
                    var CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == UserId &&
                                                                                     x.RoleId == uow.DbContext.MainRoles.
                                                                                                FirstOrDefault(x => x.Code == UserType.CommitteeManager.ToString()).ID)?.CommitteeId;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == CommitteeId).
                                                    Orders.
                                                    Where(x => x.Stage == OrderStage.Committee || x.Stage == OrderStage.DataEntry).
                                                    Where(x => orderStatus.Contains(x.OrderHistories.OrderBy(h => h.ActionDate).LastOrDefault().OrderStatus) || orderStatus.Count() == 0).
                                                    Where(x => x.Code.Contains(searchTerm) || searchTerm == null).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                     Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == CommitteeId).
                                                    Orders.
                                                    Where(x => x.Stage == OrderStage.Committee || x.Stage == OrderStage.DataEntry).
                                                    Where(x => orderStatus.Contains(x.OrderHistories.OrderBy(h => h.ActionDate).LastOrDefault().OrderStatus) || orderStatus.Count() == 0).
                                                    AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        MuslimeId = x.Muslime.ID,
                        Stage = "مدير الجمعية",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    #endregion
                    break;
                case OrderStage.Department:
                    #region Department Orders
                    var DepartmentId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == UserId &&
                                                                                 ((x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.DepartmentManager.ToString()).ID) ||
                                                                                 (x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.NegoiatedDepartmentManager.ToString()).ID)))?.
                                                                                                DepartmentId;
                    if (DepartmentId == null)
                    {
                        DepartmentId = uow.DbContext.DepartmentNegoiationUsers.FirstOrDefault(x => x.UserId == UserId)?.DepartmentId;
                    }

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    Where(x => x.DepartmentId == DepartmentId).
                                                    Where(x => x.ID == committeeId || committeeId == null).
                                                    SelectMany(x => x.Orders).
                                                    Where(x => x.Stage == OrderStage.Department).
                                                    Where(x => (x.Code.Contains(searchTerm) || searchTerm == null)).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                   Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                   Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                   Where(x => x.DepartmentId == DepartmentId).
                                                   Where(x => x.ID == committeeId || committeeId == null).
                                                   SelectMany(x => x.Orders).
                                                   Where(x => x.Stage == OrderStage.Department).
                                                   AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        Stage = "مدير إدارة الدعوة والإرشاد",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    #endregion 
                    break;
                case OrderStage.Branch:
                case OrderStage.Other:


                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                     Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    SelectMany(x => x.Orders).
                                                    Where(x => (x.Code.Contains(searchTerm) || searchTerm == null)).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                   Include(x => x.Orders).
                                                       ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                   SelectMany(x => x.Orders).
                                                   AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        Stage = "",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    break;
                default:
                    #region Technical Support Orders
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders.
                                                        Where(y => y.Stage == OrderStage.Department)).
                                                        ThenInclude(x => x.Muslime).
                                                            ThenInclude(x => x.PersonalData).
                                                      Include(x => x.Orders.
                                                        Where(y => y.Stage == OrderStage.Department)).
                                                            ThenInclude(x => x.OrderHistories).
                                                    Where(x => x.DepartmentId == _DepartmentId || _DepartmentId == null).
                                                    Where(x => x.ID == committeeId || committeeId == null).
                                                    SelectMany(x => x.Orders).
                                                    Where(x => (x.Code.Contains(searchTerm) || searchTerm == null)).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                   Include(x => x.Orders.
                                                       Where(y => y.Stage == OrderStage.Department)).
                                                       ThenInclude(x => x.Muslime).
                                                           ThenInclude(x => x.PersonalData).
                                                   Include(x => x.Orders.
                                                        Where(y => y.Stage == OrderStage.Department)).
                                                            ThenInclude(x => x.OrderHistories).
                                                   Where(x => x.DepartmentId == _DepartmentId || _DepartmentId == null).
                                                   Where(x => x.ID == committeeId || committeeId == null).
                                                   SelectMany(x => x.Orders).
                                                   AsQueryable();

                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    #endregion 
                    break;
            }
            return viewerPagination;
        }


        public ViewerPagination<OrderListDto> getOrdersForPreview(int page, int pageSize, string searchTerm, int? committeeId, int? _DepartmentId, List<OrderStatus?> orderStatus)
        {
            IQueryable<Order> myData;
            ViewerPagination<OrderListDto> viewerPagination = new ViewerPagination<OrderListDto>();
            int myDataCount = 0;
            OrderStage orderStage = OrderStage.Other;
            var UserId = uow.SessionServices.UserId;
            var Role = uow.DbContext.MainUserRole.AsNoTracking().Include(x => x.Role).FirstOrDefault(x => x.UserId == UserId)?.Role;

            UserType userType = uow.SessionServices.UserType;

            switch (userType)
            {
                case UserType.DataEntry:
                    orderStage = OrderStage.DataEntry;
                    break;
                case UserType.CommitteeManager:
                    orderStage = OrderStage.Committee;
                    break;
                case UserType.DepartmentManager:
                    orderStage = OrderStage.Department;
                    break;
                case UserType.BranchManager:
                    orderStage = OrderStage.Branch;
                    break;
                case UserType.SuperAdmin:
                    orderStage = OrderStage.Other;
                    break;
                case UserType.NegoiatedDepartmentManager:
                    orderStage = OrderStage.Department;
                    break;
                case UserType.NegoiatedBranchManager:
                    orderStage = OrderStage.Branch;
                    break;
                case UserType.None:
                    break;
                default:
                    break;
            }

            switch (orderStage)
            {
                case OrderStage.DataEntry:
                    #region DataEntry Orders
                    var _CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == UserId &&
                                                                                     x.RoleId == uow.DbContext.MainRoles.
                                                                                                FirstOrDefault(x => x.Code == UserType.DataEntry.ToString()).ID)?.
                                                                                                CommitteeId;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.DataEntry).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == _CommitteeId).
                                                    Orders.
                                                    Where(x => x.Stage != OrderStage.DataEntry).
                                                    Where(x => x.DataEntry.ID == UserId && (orderStatus.Contains(x.OrderHistories.OrderBy(h => h.ActionDate).LastOrDefault().OrderStatus) || orderStatus.Count() == 0)).
                                                    Where(x => x.Code.Contains(searchTerm) || searchTerm == null).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.DataEntry).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == _CommitteeId).
                                                    Orders.
                                                     Where(x => x.Stage != OrderStage.DataEntry).
                                                     Where(x => x.DataEntry.ID == UserId &&
                                                         (orderStatus.Contains(x.OrderHistories.OrderBy(h => h.ActionDate).LastOrDefault().OrderStatus) || orderStatus.Count() == 0)).
                                                    AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        MuslimeId = x.Muslime.ID,
                        //Stage = "موظف الجمعية",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    break;
                #endregion

                case OrderStage.Committee:
                    #region Committee Orders
                    var CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == UserId &&
                                                                                     x.RoleId == uow.DbContext.MainRoles.
                                                                                                FirstOrDefault(x => x.Code == UserType.CommitteeManager.ToString()).ID)?.
                                                                                                CommitteeId;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == CommitteeId).
                                                    Orders.
                                                    Where(x => (x.Stage != OrderStage.Committee && x.Stage != OrderStage.DataEntry) || (x.Stage == OrderStage.DataEntry && x.OrderHistories.Any(y => y.OrderStatus == OrderStatus.Reject))).
                                                    Where(x => x.Code.Contains(searchTerm) || searchTerm == null).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                     Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == CommitteeId).
                                                    Orders.
                                                    Where(x => (x.Stage != OrderStage.Committee && x.Stage != OrderStage.DataEntry) || (x.Stage == OrderStage.DataEntry && x.OrderHistories.Any(y => y.OrderStatus == OrderStatus.Reject))).
                                                    AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        //Stage = "رئيس الجمعية",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    #endregion
                    break;
                case OrderStage.Department:
                    #region Department Orders
                    var DepartmentId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == UserId &&
                                                                                 ((x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.DepartmentManager.ToString()).ID) ||
                                                                                 (x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.NegoiatedDepartmentManager.ToString()).ID)))?.
                                                                                                DepartmentId;
                    if (DepartmentId == null)
                    {
                        DepartmentId = uow.DbContext.DepartmentNegoiationUsers.FirstOrDefault(x => x.UserId == UserId)?.DepartmentId;
                    }

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    Where(x => x.DepartmentId == DepartmentId).
                                                    Where(x => x.ID == committeeId || committeeId == null).
                                                    SelectMany(x => x.Orders).
                                                    Where(x => (x.Code.Contains(searchTerm) || searchTerm == null)).
                                                    Where(x => (x.Stage != OrderStage.DataEntry && x.Stage != OrderStage.Committee && x.Stage != OrderStage.Department) ||
                                                               (x.Stage == OrderStage.DataEntry && x.OrderHistories.Any(y => y.OrderStatus == OrderStatus.Reject))
                                                                ).AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                   Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                   Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                   Where(x => x.DepartmentId == DepartmentId).
                                                   Where(x => x.ID == committeeId || committeeId == null).
                                                   SelectMany(x => x.Orders).
                                                    Where(x => (x.Stage != OrderStage.DataEntry && x.Stage != OrderStage.Committee && x.Stage != OrderStage.Department) ||
                                                               (x.Stage == OrderStage.DataEntry && x.OrderHistories.Any(y => y.OrderStatus == OrderStatus.Reject))
                                                               ).
                                                   AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        //Stage = "مدير إدارة الدعوة والإرشاد",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    #endregion 
                    break;
                case OrderStage.Branch:
                    #region Branch Orders
                    var BranchDepartmentId = uow.DbContext.MainUserRole.
                                                Include(x => x.Bransh).
                                                ThenInclude(x => x.Department).
                                                FirstOrDefault(x => x.UserId == UserId &&
                                                                    (
                                                                        (x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString()).ID) ||
                                                                        (x.RoleId == uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.NegoiatedBranchManager.ToString()).ID)
                                                                    )
                                                                    )?.Bransh?.Department?.ID;

                    if (BranchDepartmentId == null)
                    {
                        BranchDepartmentId = uow.DbContext.BranchNegoiationUsers.Include(x => x.Bransh).ThenInclude(x => x.Department)
                            .FirstOrDefault(x => x.UserId == UserId)?.Bransh.Department.ID;
                    }

                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                     Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    Where(x => x.DepartmentId == BranchDepartmentId).
                                                    Where(x => x.ID == committeeId || committeeId == null).
                                                    SelectMany(x => x.Orders).
                                                    Where(x => (x.Stage != OrderStage.DataEntry && x.Stage != OrderStage.Committee && x.Stage != OrderStage.Department && x.Stage != OrderStage.Branch) ||
                                                               (x.Stage == OrderStage.DataEntry && x.OrderHistories.Any(y => y.OrderStatus == OrderStatus.Reject))
                                                               ).Where(x => (x.Code.Contains(searchTerm) || searchTerm == null)).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                   Include(x => x.Orders).
                                                       ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                   Where(x => x.DepartmentId == BranchDepartmentId).
                                                   Where(x => x.ID == committeeId || committeeId == null).
                                                   SelectMany(x => x.Orders).
                                                    Where(x => (x.Stage != OrderStage.DataEntry && x.Stage != OrderStage.Committee && x.Stage != OrderStage.Department && x.Stage != OrderStage.Branch) ||
                                                               (x.Stage == OrderStage.DataEntry && x.OrderHistories.Any(y => y.OrderStatus == OrderStatus.Reject))
                                                               ).AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        //Stage = " مدير فرع الوزارة بالمنطقة ",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    #endregion 
                    break;
                default:
                    #region Technical Support Orders
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders.
                                                        Where(y => y.Stage == OrderStage.Department)).
                                                        ThenInclude(x => x.Muslime).
                                                            ThenInclude(x => x.PersonalData).
                                                      Include(x => x.Orders.
                                                        Where(y => y.Stage == OrderStage.Department)).
                                                            ThenInclude(x => x.OrderHistories).
                                                    Where(x => x.DepartmentId == _DepartmentId || _DepartmentId == null).
                                                    Where(x => x.ID == committeeId || committeeId == null).
                                                    SelectMany(x => x.Orders).
                                                    Where(x => (x.Code.Contains(searchTerm) || searchTerm == null)).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                   Include(x => x.Orders.
                                                       Where(y => y.Stage == OrderStage.Department)).
                                                       ThenInclude(x => x.Muslime).
                                                           ThenInclude(x => x.PersonalData).
                                                   Include(x => x.Orders.
                                                        Where(y => y.Stage == OrderStage.Department)).
                                                            ThenInclude(x => x.OrderHistories).
                                                   Where(x => x.DepartmentId == _DepartmentId || _DepartmentId == null).
                                                   Where(x => x.ID == committeeId || committeeId == null).
                                                   SelectMany(x => x.Orders).
                                                   AsQueryable();

                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    #endregion 
                    break;
            }
            return viewerPagination;
        }

        public ViewerPagination<OrderListDto> getFinishedOrders(int page, int pageSize, string searchTerm, int? committeeId, int? _DepartmentId)
        {
            IQueryable<Order> myData;
            ViewerPagination<OrderListDto> viewerPagination = new ViewerPagination<OrderListDto>();
            int myDataCount = 0;
            OrderStage orderStage;
            var UserId = uow.SessionServices.UserId;
            var Role = uow.DbContext.MainUserRole.Include(x => x.Role).FirstOrDefault(x => x.UserId == UserId)?.Role;

            if (Role?.Code == UserType.DataEntry.ToString()) orderStage = OrderStage.DataEntry;
            else if (Role?.Code == UserType.CommitteeManager.ToString()) orderStage = OrderStage.DataEntry;
            else if (Role?.Code == UserType.DepartmentManager.ToString()) orderStage = OrderStage.Department;
            else if (Role?.Code == UserType.BranchManager.ToString()) orderStage = OrderStage.Branch;
            else orderStage = OrderStage.Other;
            switch (orderStage)
            {
                case OrderStage.DataEntry:
                    #region DataEntry Orders
                    /*
                    x.RoleId == uow.DbContext.MainRoles.
                               FirstOrDefault(x => x.Name == UserType.DataEntry.ToString()).ID
                    */
                    var _CommitteeId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.UserId == UserId)?.CommitteeId;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.DataEntry).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == _CommitteeId).
                                                    Orders.
                                                    Where(x => x.Stage == OrderStage.ReadyToPrintCard).
                                                    //Where(x => x.DataEntry.ID == UserId).
                                                    Where(x => x.Code.Contains(searchTerm) || searchTerm == null).
                                                    AsQueryable();
                    }
                    else
                    {
                        myData = uow.DbContext.Committees.
                                                    Include(x => x.Orders).ThenInclude(x => x.Muslime).ThenInclude(x => x.PersonalData).
                                                    Include(x => x.Orders).ThenInclude(x => x.DataEntry).
                                                    Include(x => x.Orders).ThenInclude(x => x.OrderHistories).
                                                    FirstOrDefault(x => x.ID == _CommitteeId).
                                                    Orders.
                                                    Where(x => x.Stage == OrderStage.ReadyToPrintCard).
                                                    //Where(x => x.DataEntry.ID == UserId).
                                                    AsQueryable();
                    }
                    myDataCount = myData.Count();
                    viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new OrderListDto
                    {
                        OrderCode = x.Code,
                        CreationDate = x.CreationDate,
                        MuslimeName = x.Muslime.PersonalData.NameBeforeFristAr + " " +
                                        x.Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          x.Muslime.PersonalData.NameBeforeLastAr,
                        MuslimeId = x.Muslime.ID,
                        Stage = "موظف الجمعية",
                        OrderTimeLine = x.OrderHistories.Select(item => new OrderHistoryDto
                        {
                            ActionDate = item.ActionDate.ToShortDateString(),
                            Description = item.Description,
                            DataEntryName = item.UserName,
                            OrderStatus = item.OrderStatus
                        }).ToList()
                    }).ToList();
                    viewerPagination.OriginalListListCount = myDataCount;
                    break;
                    #endregion
            }
            return viewerPagination;
        }


        public async Task<MuslimeDto> getData(string OrderCode)
        {
            try
            {
                MuslimeDto muslime = null;
                PersonalDataDto personalDataDto = null;
                PersonalInformationDto PersonalInformationDto = null;
                AttachmentDto attachmentDto = null;

                var order = uow.DbContext.Orders.Include(x => x.Muslime).FirstOrDefault(x => x.Code == OrderCode);

                if (order != null)
                {
                    int MuslimeId = order.Muslime.ID;
                    Muslime Muslime = uow.Muslime.DbSet.
                    Include(x => x.PersonalData).ThenInclude(x => x.Witness).
                    Include(x => x.PersonalData).ThenInclude(x => x.PreacherName).
                    Include(x => x.PersonalInformation).
                    ThenInclude(x => x.ResidenceIssuePlace).
                    Include(x => x.PersonalInformation).
                    ThenInclude(x => x.EducationalLevel).
                    Include(x => x.PersonalInformation).
                    ThenInclude(x => x.PreviousReligion).
                    Include(x => x.ContactData).
                    Include(x => x.Work).
                    Include(x => x.IsslamRecognition).
                    Include(x => x.OriginalCountry).ThenInclude(x => x.Country).
                    Include(x => x.Attachment).
                    Include(x => x.CurrentResidence).
                    Include(x => x.FamilyInformation).
                    FirstOrDefault(x => x.ID == MuslimeId);

                    if (Muslime.PersonalData != null)
                    {
                        //IslamDateHijry = Muslime.PersonalData.IslamDateHijry,
                        List<Witness> Witness = Muslime.PersonalData.Witness.OrderBy(x => x.ID).ToList();
                        personalDataDto = new PersonalDataDto
                        {
                            NameBeforeFristAr = Muslime.PersonalData.NameBeforeFristAr,
                            NameBeforeMiddleAr = Muslime.PersonalData.NameBeforeMiddleAr,
                            NameBeforeLastAr = Muslime.PersonalData.NameBeforeLastAr,
                            NameAfter = Muslime.PersonalData.NameAfter,
                            NameBeforeFristEn = Muslime.PersonalData.NameBeforeFristEn,
                            NameBeforeMiddleEn = Muslime.PersonalData.NameBeforeMiddleEn,
                            IslamDate = Muslime.PersonalData.IslamDate,
                            NameAfterEn = Muslime.PersonalData.NameAfterEn,
                            PreacherName = Muslime.PersonalData.PreacherName,
                            NameBeforeLastEn = Muslime.PersonalData.NameBeforeLastEn,
                            FirstWitness = new WitnessDto { Name = Witness.FirstOrDefault().Name, ID = Witness.FirstOrDefault().ID },
                            SecondWitness = Witness.Skip(1).FirstOrDefault() != null ? new WitnessDto { Name = Witness.Skip(1).FirstOrDefault()?.Name, ID = Witness.Skip(1).FirstOrDefault().ID } : null
                        };
                    }

                    if (Muslime.PersonalInformation != null)
                    {
                        PersonalInformationDto = new PersonalInformationDto
                        {
                            DateOfBirth = Muslime.PersonalInformation.DateOfBirth,
                            PlaceOfBirth = Muslime.PersonalInformation.PlaceOfBirth,
                            Nationality = Muslime.PersonalInformation.Nationality,
                            Gender = Muslime.PersonalInformation.Gender,
                            PositionInFamily = Muslime.PersonalInformation.PositionInFamily,
                            MaritalStatus = Muslime.PersonalInformation.MaritalStatus,
                            HusbandName = Muslime.PersonalInformation.HusbandName,
                            EducationalLevel = Muslime.PersonalInformation.EducationalLevel,
                            PreviousReligion = Muslime.PersonalInformation.PreviousReligion,
                            ResidenceNumber = Muslime.PersonalInformation.ResidenceNumber,
                            ResidenceIssueDate = Muslime.PersonalInformation.ResidenceIssueDate,
                            ResidenceIssuePlace = Muslime.PersonalInformation.ResidenceIssuePlace,
                            DateOfEntryKingdom = Muslime.PersonalInformation.DateOfEntryKingdom,
                            DateOfPassportIssue = Muslime.PersonalInformation.DateOfPassportIssue,
                            PassportNumber = Muslime.PersonalInformation.PassportNumber,
                            PlaceOfPassportIssue = Muslime.PersonalInformation.PlaceOfPassportIssue
                        };
                    }
                    if (Muslime.Attachment != null)
                    {
                        attachmentDto = new AttachmentDto
                        {
                            _Personal = Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Personal)?.AttachmentValue != null
                                        ? Convert.ToBase64String(Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Personal).AttachmentValue)
                                        : null,
                            _Passport = Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Passport)?.AttachmentValue != null
                                        ? Convert.ToBase64String(Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Passport).AttachmentValue)
                                        : null,
                            _Accomodation = Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Accommodation)?.AttachmentValue != null
                                        ? Convert.ToBase64String(Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Accommodation).AttachmentValue)
                                        : null,
                        };
                    }


                    muslime = new MuslimeDto
                    {
                        PersonalData = personalDataDto,
                        PersonalInformation = PersonalInformationDto,
                        ContactData = new ContactAndInfoDataViewModel
                        {
                            ContactData = Muslime.ContactData,
                            OriginalCountry = Muslime.OriginalCountry,
                            CurrentResidence = Muslime.CurrentResidence
                        },
                        FamilyInformation = new FamilyAndWorkDto
                        {
                            FamilyInformation = Muslime.FamilyInformation,
                            Work = Muslime.Work
                        },
                        IsslamRecognition = new IslamRecognitionWayDto
                        {
                            IslamRecognitionWay = Muslime.IsslamRecognition.Select(x => new IsslamRecognitionData { ID = x.ID, Title = x.Title }).ToList()
                        },
                        Attachment = attachmentDto

                    };
                }


                return muslime;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<UserDataForVieweing> getDataForQuery(string Id)
        {
            try
            {
                UserDataForVieweing result = null;
                string _value = Id.Replace("_", "=");
                var DecryptId = EncryptHelper.Decrypt(Id);
                if (string.IsNullOrEmpty(DecryptId))
                    return result;

                int value = int.Parse(DecryptId);

                MuslimeDto muslime = null;
                PersonalDataDto personalDataDto = null;
                PersonalInformationDto PersonalInformationDto = null;

                var Muslime = await uow.DbContext.Muslimes.
                                          Include(x => x.PersonalData).
                                          Include(x => x.PersonalInformation).ThenInclude(x => x.PreviousReligion).
                                          Include(x => x.PersonalInformation).ThenInclude(x => x.Nationality).
                                          Include(x => x.ContactData).
                                          Include(x => x.OriginalCountry).
                                          FirstOrDefaultAsync(x => x.ID == value);

                if (Muslime != null)
                {
                    result = new UserDataForVieweing
                    {
                        NameBeforeAr = $"{Muslime.PersonalData.NameBeforeFristAr} {Muslime.PersonalData.NameBeforeMiddleAr} {Muslime.PersonalData.NameBeforeLastAr}",
                        NameAfter = Muslime.PersonalData.NameAfter,
                        NameAfterEn = Muslime.PersonalData.NameAfterEn,
                        Nationality = Muslime.PersonalInformation.Nationality.NameAr,
                        DateOfEntryKingdom = Muslime.PersonalInformation.DateOfEntryKingdom,
                        PassportNumber = Muslime.PersonalInformation.PassportNumber,
                        IslamDate = Muslime.PersonalData.IslamDate,
                        PlaceOfBirth = Muslime.PersonalInformation.PlaceOfBirth,
                        City = Muslime?.OriginalCountry?.City,
                        Region = Muslime?.OriginalCountry?.Region,
                        Street = Muslime?.OriginalCountry?.Street,
                        ResidenceNumber = Muslime.PersonalInformation.ResidenceNumber,
                        PreviousReligion = Muslime.PersonalInformation.PreviousReligion.Title
                    };
                }


                return result;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public async Task<PersonalDataDto> getPersonalData(int MuslimeId)
        {
            try
            {
                PersonalDataDto personalDataDto = null;
                Muslime Muslime = await uow.DbContext.Muslimes.
                    Include(x => x.PersonalData).ThenInclude(x => x.Witness).
                    Include(x => x.PersonalData).ThenInclude(x => x.PreacherName)
                    .FirstOrDefaultAsync(x => x.ID == MuslimeId);
                if (Muslime != null)
                {
                    if (Muslime.PersonalData != null)
                    {
                        List<Witness> Witness = Muslime.PersonalData.Witness.OrderBy(x => x.ID).ToList();
                        personalDataDto = new PersonalDataDto
                        {
                            NameBeforeFristAr = Muslime.PersonalData.NameBeforeFristAr,
                            NameBeforeMiddleAr = Muslime.PersonalData.NameBeforeMiddleAr,
                            NameBeforeLastAr = Muslime.PersonalData.NameBeforeLastAr,
                            NameAfter = Muslime.PersonalData.NameAfter,
                            NameBeforeFristEn = Muslime.PersonalData.NameBeforeFristEn,
                            NameBeforeMiddleEn = Muslime.PersonalData.NameBeforeMiddleEn,
                            IslamDate = Muslime.PersonalData.IslamDate,
                            NameAfterEn = Muslime.PersonalData.NameAfterEn,
                            PreacherName = Muslime.PersonalData.PreacherName,
                            NameBeforeLastEn = Muslime.PersonalData.NameBeforeLastEn,
                            FirstWitness = new WitnessDto { Name = Witness.FirstOrDefault().Name, ID = Witness.FirstOrDefault().ID },
                            SecondWitness = Witness.Skip(1).FirstOrDefault() != null ? new WitnessDto { Name = Witness.Skip(1).FirstOrDefault()?.Name, ID = Witness.Skip(1).FirstOrDefault().ID } : null
                        };
                    }

                }
                return personalDataDto;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<PersonalInformationDto> getPersonalInformation(int MuslimeId)
        {
            try
            {
                PersonalInformationDto PersonalInformationDto = null;
                Muslime Muslime = uow.Muslime.DbSet.
                    Include(x => x.PersonalInformation).ThenInclude(x => x.EducationalLevel)
                    .Include(x => x.PersonalInformation).ThenInclude(x => x.Nationality)
                    .Include(x => x.PersonalInformation).ThenInclude(x => x.PreviousReligion)
                    .Include(x => x.PersonalInformation).ThenInclude(x => x.ResidenceIssuePlace)
                    .FirstOrDefault(x => x.ID == MuslimeId);

                if (Muslime != null)
                {
                    if (Muslime.PersonalInformation != null)
                    {
                        PersonalInformationDto = new PersonalInformationDto
                        {
                            DateOfBirth = Muslime.PersonalInformation.DateOfBirth,
                            PlaceOfBirth = Muslime.PersonalInformation.PlaceOfBirth,
                            Nationality = Muslime.PersonalInformation.Nationality,
                            Gender = Muslime.PersonalInformation.Gender,
                            PositionInFamily = Muslime.PersonalInformation.PositionInFamily,
                            MaritalStatus = Muslime.PersonalInformation.MaritalStatus,
                            HusbandName = Muslime.PersonalInformation.HusbandName,
                            EducationalLevel = Muslime.PersonalInformation.EducationalLevel,
                            PreviousReligion = Muslime.PersonalInformation.PreviousReligion,
                            ResidenceNumber = Muslime.PersonalInformation.ResidenceNumber,
                            ResidenceIssueDate = Muslime.PersonalInformation.ResidenceIssueDate,
                            ResidenceIssuePlace = Muslime.PersonalInformation.ResidenceIssuePlace,
                            DateOfEntryKingdom = Muslime.PersonalInformation.DateOfEntryKingdom,
                            DateOfPassportIssue = Muslime.PersonalInformation.DateOfPassportIssue,
                            PassportNumber = Muslime.PersonalInformation.PassportNumber,
                            PlaceOfPassportIssue = Muslime.PersonalInformation.PlaceOfPassportIssue
                        };
                    }
                }
                return PersonalInformationDto;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<FamilyAndWorkDto> getFamilyAndWork(int MuslimeId)
        {
            try
            {
                FamilyAndWorkDto FamilyAndWorkDto = null;
                Muslime Muslime = await uow.DbContext.Muslimes.Include(x => x.FamilyInformation).Include(r => r.Work).FirstOrDefaultAsync(x => x.ID == MuslimeId);
                if (Muslime != null)
                {
                    FamilyAndWorkDto = new FamilyAndWorkDto
                    {
                        FamilyInformation = Muslime.FamilyInformation,
                        Work = Muslime.Work
                    };
                }
                return FamilyAndWorkDto;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public async Task<AttachmentDto> getattachmentDto(int MuslimeId)
        {
            try
            {
                AttachmentDto attachmentDto = null;
                Muslime Muslime = await uow.Muslime.DbSet.
                                        Include(x => x.Attachment).
                                        FirstOrDefaultAsync(x => x.ID == MuslimeId);
                if (Muslime != null)
                {
                    if (Muslime.Attachment != null && Muslime.Attachment.Count() > 0)
                    {
                        attachmentDto = new AttachmentDto
                        {
                            _Personal = Convert.ToBase64String(Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Personal).AttachmentValue),
                            _Passport = Convert.ToBase64String(Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Passport).AttachmentValue),
                            _Accomodation = Convert.ToBase64String(Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Accommodation).AttachmentValue),
                        };
                    }
                }
                return attachmentDto;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<ContactAndInfoDataViewModel> getContactData(int MuslimeId)
        {
            try
            {
                ContactAndInfoDataViewModel ContactData = null;
                Muslime Muslime = await uow.Muslime.DbSet.
                                        Include(x => x.ContactData).
                                        Include(x => x.OriginalCountry).ThenInclude(x => x.Country).
                                        Include(x => x.CurrentResidence).
                                        FirstOrDefaultAsync(x => x.ID == MuslimeId);
                if (Muslime != null)
                {
                    ContactData = new ContactAndInfoDataViewModel
                    {
                        ContactData = Muslime.ContactData,
                        OriginalCountry = Muslime.OriginalCountry,
                        CurrentResidence = Muslime.CurrentResidence
                    };
                }
                return ContactData;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<IsslamRecognitionData>> getIslamRecognitionWay(int MuslimeId)
        {
            try
            {
                List<IsslamRecognitionData> IsslamRecognitionData = null;
                Muslime Muslime = await uow.DbContext.Muslimes.Include(x => x.IsslamRecognition).FirstOrDefaultAsync(x => x.ID == MuslimeId);
                if (Muslime != null)
                {
                    IsslamRecognitionData = Muslime.IsslamRecognition.Select(x => new IsslamRecognitionData
                    {
                        ID = x.ID,
                        Title = x.Title
                    }).ToList();

                }
                return IsslamRecognitionData;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public ViewerPagination<DepartmentDto> getAllDepartment(int page, int pageSize, string searchTerm)
        {
            var UserId = uow.SessionServices.UserId;
            var LoggedInUser = uow.DbContext.MainUsers.AsNoTracking().FirstOrDefault(x => x.ID == UserId);

            MainRole Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.DepartmentManager.ToString());
            IQueryable<Department> myData;
            int myDataCount = 0;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                myData = uow.DbContext.Department.Where(x => (LoggedInUser.UserType == UserType.SuperAdmin) || (x.BranshID == LoggedInUser.ID)).Where(a => a.Title.Contains(searchTerm));
            }
            else
            {
                myData = uow.DbContext.Department.Where(x => (LoggedInUser.UserType == UserType.SuperAdmin) || (x.BranshID == LoggedInUser.ID));
            }
            myDataCount = myData.Count();
            ViewerPagination<DepartmentDto> viewerPagination = new ViewerPagination<DepartmentDto>();
            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new DepartmentDto
            {
                ID = x.ID,
                Title = x.Title,
                Manager = new NameIdViewModel
                {
                    ID = uow.DbContext.MainUserRole
                                        .Where(y => y.DepartmentId == x.ID && y.RoleId == Role.ID)
                                        .Select(y => y.UserId)
                                        .FirstOrDefault(),

                    Title = uow.DbContext.MainUserRole
                                            .Include(u => u.User)
                                            .Where(y => y.DepartmentId == x.ID && y.RoleId == Role.ID)
                                            .Select(y => y.User.Name)
                                            .FirstOrDefault()
                }


            }).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }

        public ViewerPagination<BranshListDto> GetAllBranchs(int page, int pageSize, string searchTerm)
        {
            var UserId = uow.SessionServices.UserId;
            var LoggedInUser = uow.DbContext.MainUsers.AsNoTracking().FirstOrDefault(x => x.ID == UserId);

            MainRole Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString());

            IQueryable<MinistryBransh> myData;
            int myDataCount = 0;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                myData = uow.DbContext.MinistryBranshs.Where(x => (LoggedInUser.UserType == UserType.SuperAdmin) || (x.ID == LoggedInUser.BranchId)).Where(a => a.Title.Contains(searchTerm));
            }
            else
            {
                myData = uow.DbContext.MinistryBranshs.Where(x => (LoggedInUser.UserType == UserType.SuperAdmin) || (x.ID == LoggedInUser.BranchId));
            }
            myDataCount = myData.Count();
            ViewerPagination<BranshListDto> viewerPagination = new ViewerPagination<BranshListDto>();
            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new BranshListDto
            {
                ID = x.ID,
                Title = x.Title,
                Manager = new NameIdViewModel
                {
                    ID = uow.DbContext.MainUserRole
                                        .Where(y => y.BranshId == x.ID && y.RoleId == Role.ID)
                                        .Select(y => y.UserId)
                                        .FirstOrDefault(),

                    Title = uow.DbContext.MainUserRole
                                            .Include(u => u.User)
                                            .Where(y => y.BranshId == x.ID && y.RoleId == Role.ID)
                                            .Select(y => y.User.Name)
                                            .FirstOrDefault()
                }
            }).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }

        public ViewerPagination<ResidenceIssuePlace> GetResidencePalcePaginated(int page, int pageSize, string searchTerm)
        {
            IQueryable<ResidenceIssuePlace> myData;
            int myDataCount = 0;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                myData = uow.DbContext.ResidenceIssuePlace.Where(a => a.Title.Contains(searchTerm));
            }
            else
            {
                myData = uow.DbContext.ResidenceIssuePlace;
            }
            myDataCount = myData.Count();
            ViewerPagination<ResidenceIssuePlace> viewerPagination = new ViewerPagination<ResidenceIssuePlace>();
            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }

        public ViewerPagination<Preacher> GetPreshersPaginated(int page, int pageSize, string searchTerm)
        {
            IQueryable<Preacher> myData;
            int myDataCount = 0;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                myData = uow.DbContext.Preachers.Where(a => a.Title.Contains(searchTerm) || a.Identity.Contains(searchTerm) || a.ContactNumber.Contains(searchTerm));
            }
            else
            {
                myData = uow.DbContext.Preachers;
            }
            myDataCount = myData.Count();
            ViewerPagination<Preacher> viewerPagination = new ViewerPagination<Preacher>();
            viewerPagination.PaginationList = myData.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            viewerPagination.OriginalListListCount = myDataCount;
            return viewerPagination;
        }

        public PrintCardView PrintCard(string Code)
        {
            try
            {
                PrintCardView resultFront = new();
                PrintCardView resultBack = new();
                PrintCardView FinalResult = new();

                var order = uow.DbContext.Orders.
                    Include(x => x.Committee).
                    ThenInclude(x => x.Department).
                    Include(x => x.Muslime).
                    FirstOrDefault(x => x.Code == Code);
                if (order != null)
                {
                    var BranshID = order.Committee.Department.BranshID;
                    var Role = uow.DbContext.MainRoles.FirstOrDefault(x => x.Code == UserType.BranchManager.ToString());
                    var ManagerId = uow.DbContext.MainUserRole.FirstOrDefault(x => x.BranshId == BranshID && x.RoleId == Role.ID)?.UserId;
                    int MuslimeId = order.Muslime.ID;
                    Muslime Muslime = uow.Muslime.DbSet.
                    Include(x => x.PersonalData).
                    Include(x => x.PersonalInformation).ThenInclude(x => x.Nationality).
                    Include(x => x.Attachment).
                    FirstOrDefault(x => x.ID == MuslimeId);

                    string reportPath = Path.Combine(uow.ContentRootPath, "Templates", "Card.rdlc");
                    string CardBackPath = Path.Combine(uow.ContentRootPath, "Templates", "CardBack.rdlc");
                    Stream CardFrontDefinition;
                    Stream CardBackDefinition;

                    using var fs_CardFront = new System.IO.FileStream(reportPath, FileMode.Open);
                    using var fs_CardBack = new System.IO.FileStream(CardBackPath, FileMode.Open);

                    CardFrontDefinition = fs_CardFront;
                    CardBackDefinition = fs_CardBack;

                    LocalReport CardFront = new LocalReport();
                    LocalReport CardBack = new LocalReport();

                    CardFront.EnableExternalImages = true;
                    CardBack.EnableExternalImages = true;

                    CardFront.LoadReportDefinition(CardFrontDefinition);
                    CardBack.LoadReportDefinition(CardBackDefinition);

                    var dataTable = new DataTable();
                    dataTable.Columns.Add("NameAfter", typeof(string));
                    dataTable.Columns.Add("CountryAr", typeof(string));
                    dataTable.Columns.Add("CountryEn", typeof(string));
                    dataTable.Columns.Add("PassportNumber", typeof(string));
                    dataTable.Columns.Add("NameAfterEn", typeof(string));
                    dataTable.Columns.Add("IslamDate", typeof(string));
                    dataTable.Columns.Add("NameBeforeEn", typeof(string));
                    dataTable.Columns.Add("NameBeforeAr", typeof(string));
                    dataTable.Columns.Add("Branch", typeof(string));
                    dataTable.Columns.Add("Qr", typeof(byte[]));
                    dataTable.Columns.Add("Image", typeof(byte[]));
                    dataTable.Columns.Add("Signature", typeof(byte[]));

                    var row = dataTable.NewRow();

                    row["NameAfter"] = Muslime.PersonalData.NameAfter.ToString();
                    row["CountryAr"] = Muslime.PersonalInformation.Nationality.NameAr.ToString();
                    row["CountryEn"] = Muslime.PersonalInformation.Nationality.NameEn.ToString();
                    row["PassportNumber"] = Muslime.PersonalInformation.PassportNumber != null ? Muslime.PersonalInformation.PassportNumber.ToString() : Muslime.PersonalInformation.ResidenceNumber.ToString();
                    row["NameAfterEn"] = Muslime.PersonalData.NameAfterEn;
                    row["IslamDate"] = Muslime.PersonalData.IslamDate.ToString("dd/MM/yyyy");
                    row["NameBeforeEn"] = Muslime.PersonalData.NameBeforeFristEn + " " +
                                          Muslime.PersonalData.NameBeforeMiddleEn + " " +
                                          Muslime.PersonalData.NameBeforeLastEn;

                    row["NameBeforeAr"] = Muslime.PersonalData.NameBeforeFristAr + " " +
                                          Muslime.PersonalData.NameBeforeMiddleAr + " " +
                                          Muslime.PersonalData.NameBeforeLastAr;
                    row["Branch"] = "";

                    var request = uow._httpContextAccessor.HttpContext.Request;
                    string _value = EncryptHelper.Encrypt(order.Muslime.ID.ToString());
                    var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}/#/query-info/details/{_value.Replace("=", "_")}";

                    row["Qr"] = GenerateQRCode(baseUrl);
                    row["Image"] = (Muslime.Attachment.FirstOrDefault(x => x.ImageType == ImageType.Personal).AttachmentValue);
                    row["Signature"] = uow.DbContext.MainUsers.
                                                        Include(x => x.Attachment).
                                                        FirstOrDefault(x => x.ID == ManagerId)?.Attachment.AttachmentValue;
                    dataTable.Rows.Add(row);

                    ReportDataSource rds = new ReportDataSource("DataSet1", dataTable);
                    CardFront.DataSources.Add(rds);
                    CardBack.DataSources.Add(rds);

                    byte[] pdf = CardFront.Render("PDF");
                    byte[] pdf_Back = CardBack.Render("PDF");

                    fs_CardFront.Dispose();
                    fs_CardBack.Dispose();

                    string base64StringFront = Convert.ToBase64String(pdf);
                    string FileSrcFront = $"data:application/pdf;base64,{base64StringFront}";
                    resultFront.file = FileSrcFront;
                    resultFront.Filename = Muslime.PersonalData.NameAfter;

                    string base64StringBack = Convert.ToBase64String(pdf_Back);
                    string FileSrcBack = $"data:application/pdf;base64,{base64StringBack}";
                    resultBack.file = FileSrcBack;


                    PdfDocument mergedDocument = new PdfDocument();

                    byte[] resultFrontBytes = Convert.FromBase64String(base64StringFront);
                    using (MemoryStream stream = new MemoryStream(resultFrontBytes))
                    {
                        PdfDocument pdfDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                        foreach (PdfPage page in pdfDocument.Pages)
                        {
                            mergedDocument.AddPage(page);
                        }
                    }

                    byte[] resultBackBytes = Convert.FromBase64String(base64StringBack);
                    using (MemoryStream stream = new MemoryStream(resultBackBytes))
                    {
                        PdfDocument pdfDocument = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
                        foreach (PdfPage page in pdfDocument.Pages)
                        {
                            mergedDocument.AddPage(page);
                        }
                    }

                    using (MemoryStream mergedStream = new MemoryStream())
                    {
                        mergedDocument.Save(mergedStream);
                        byte[] mergedBytes = mergedStream.ToArray();

                        // Return the merged file
                        string base64String = Convert.ToBase64String(mergedBytes);
                        //data:application/pdf;base64,
                        string FileSrc = $"{base64String}";
                        FinalResult.Filename = resultFront.Filename;
                        FinalResult.file = FileSrc;
                        return FinalResult;
                    }
                }
            }
            catch (Exception ex)
            {
                uow.DbContext.Exceptions.Add(new Exceptions
                {
                    Message = ex.Message,
                    Stacktrace = ex.StackTrace
                });
                uow.SaveChanges();
                return null;
            }

            return null;
        }


        public byte[] ResizeImage(byte[] imageData, int newWidth, int newHeight)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                using (Image originalImage = Image.FromStream(ms, false, false))
                {
                    using (MemoryStream msResized = new MemoryStream())
                    {
                        using (Bitmap resizedImage = new Bitmap(newWidth, newHeight))
                        {
                            using (Graphics g = Graphics.FromImage(resizedImage))
                            {
                                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                            }

                            // Save the resized image directly to the MemoryStream
                            resizedImage.Save(msResized, originalImage.RawFormat);
                        }

                        return msResized.ToArray();
                    }
                }
            }
        }

        //public byte[] ResizeImage(byte[] imageData, int newWidth, int newHeight)
        //{

        //    // Step 2: Create Image from byte array
        //    using (MemoryStream ms = new MemoryStream(imageData))
        //    {
        //        Image originalImage = Image.FromStream(ms);

        //        // Step 3: Set desired width and height
        //        Image resizedImage = new Bitmap(newWidth, newHeight);
        //        using (Graphics g = Graphics.FromImage(resizedImage))
        //        {
        //            g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
        //        }

        //        // Step 4: Convert Image back to byte array
        //        using (MemoryStream msResized = new MemoryStream())
        //        {
        //            resizedImage.Save(msResized, originalImage.RawFormat);
        //            return msResized.ToArray();
        //        }
        //    }
        //}


        public byte[] GenerateQRCode(string data)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(20); // Adjust the size as needed

            using (MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

    }
}
