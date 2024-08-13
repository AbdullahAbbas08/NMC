using Moia.Shared.Models;
using Moia.Shared.ViewModels.DTOs;
namespace Moia.BL.AutoMapper
{
    public class Profiles: Profile
    {
        public Profiles()
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<MinistryBransh, MinistryBranshDto>().
                ForMember(dest => dest.Department, opt => opt.Ignore());
            CreateMap<PersonalData, PersonalDataDto>().ReverseMap();
            CreateMap<Muslime, MuslimeDto>().ReverseMap();
            CreateMap<PersonalInformation, PersonalInformationDto>().ReverseMap();
            CreateMap<CurrentResidence, CurrentResidenceDto>().ReverseMap();
            CreateMap<ContactData, ContactDataDto>().ReverseMap();
            CreateMap<FamilyInformation, FamilyInformationDto>().ReverseMap();
            CreateMap<Work, WorkDto>().ReverseMap();
            CreateMap<MinistryBransh, BranshDto>().ReverseMap();
            CreateMap<IsslamRecognition, IsslamRecognitionDto>().ReverseMap();
            CreateMap<Attachment, AttachmentDto>().ReverseMap();
            CreateMap<Witness, WitnessDto>().ReverseMap();
            CreateMap<MainUser, MainUserForInsertDto>().ReverseMap();
            CreateMap<MainUser, MainUserDto>().
                ForMember(dest => dest.UserType, opt => opt.Ignore()).
                ForMember(dest => dest.Attachment, opt => opt.Ignore()).
                ForMember(dest => dest.Committee, opt => opt.Ignore()).
                ForMember(dest => dest.Department, opt => opt.Ignore()).
                ForMember(dest => dest.Orders, opt => opt.Ignore());
           
            CreateMap<MainUser, MainUserNameIDList>().ReverseMap();
            //CreateMap<Committee, CommitteeDto>().ForMember(d => d.DepartmentId, s => s.MapFrom(s => s.Department.ID)).ReverseMap();
            CreateMap<Committee, CommitteeList>().ReverseMap();
            CreateMap<Committee, CommitteeDto>().ReverseMap();
            CreateMap<OriginalCountry, OriginalCountryDto>().ForMember(d => d.CountryId, s => s.MapFrom(s => s.Country.ID)).ReverseMap();

            ////CreateMap<Country, CountryDto>().ForMember(d => d.NationId, s => s.MapFrom(s => s.Nation.Id));
            //CreateMap<Candidate, CandidateDto>();
            //CreateMap<_Year, _YearDto>();
            //CreateMap<User, UserDetailsDTO>();

            //CreateMap<UserRole, UserRoleDto>()
            //    .ForMember(d=>d.RoleNameAr,s=>s.MapFrom(s=>s.Role.RoleNameAr))
            //    .ForMember(d=>d.RoleNameEn,s=>s.MapFrom(s=>s.Role.RoleNameEn));

            //CreateMap<Localization, LocalizationDetailsDTO>();
        }
    }
}
