using Moia.Shared.ViewModels.DTOs;

namespace Moia.Shared.Models
{
    public class Muslime : _Model
    {
        public Muslime() { }
        public virtual PersonalData PersonalData { get; set; }
        public virtual PersonalInformation PersonalInformation { get; set; }
        public virtual OriginalCountry OriginalCountry { get; set; }
        public virtual CurrentResidence CurrentResidence { get; set; }
        public virtual ContactData ContactData { get; set; }
        public virtual FamilyInformation FamilyInformation { get; set; }
        public virtual Work Work { get; set; }
        public virtual List<IsslamRecognition> IsslamRecognition { get; set; }
        public virtual List<Attachment> Attachment  { get; set; }
        //public virtual Order Order { get; set; }
    }
    
   




    public class PersonalData : _Model
    {
        public string NameBeforeFristAr { get; set; }
        public string NameBeforeMiddleAr { get; set; }
        public string NameBeforeLastAr { get; set; }
        public string NameAfter { get; set; }
        public string NameBeforeFristEn { get; set; }
        public string NameBeforeMiddleEn { get; set; }
        public string NameBeforeLastEn { get; set; }
        public DateTime IslamDate { get; set; }
        //public DateTime IslamDateHijry { get; set; }
        public string NameAfterEn { get; set; }
        public Preacher PreacherName { get; set; }
        public virtual ICollection<Witness> Witness { get; set; }

      
    }
    
   

    public class PersonalInformation : _Model
    {
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfEntryKingdom { get; set; }
        public string PlaceOfBirth { get; set; }
        public Country Nationality { get; set; }
        public Gender Gender { get; set; }
        public Religion PreviousReligion { get; set; }

        public int? PositionInFamily { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public string HusbandName { get; set; }
        public EducationalLevel EducationalLevel { get; set; }
        public string ResidenceNumber { get; set; }
        public DateTime ResidenceIssueDate { get; set; }
        public ResidenceIssuePlace ResidenceIssuePlace { get; set; }
        public string PassportNumber { get; set; }
        public DateTime DateOfPassportIssue { get; set; }
        public string PlaceOfPassportIssue { get; set; }
    }
    
  

    public class CurrentResidence : _Model
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string DoorNumber { get; set; }
        public string EmergencyNumber { get; set; }
    }
    
  

    public class OriginalCountry : _Model
    {
        public virtual Country Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string DoorNumber { get; set; }
    }
    
   

    public class ContactData : _Model
    {
        [Required]
        public string PhoneNumber { get; set; }
        public string HomeNumber { get; set; }
        public string WorkNumber { get; set; }
        public string Email { get; set; }
    }

    public class FamilyInformation : _Model
    {
        public int? MembersNumber { get; set; }
        public int? BoysNumber { get; set; }
        public int? GirlsNumber { get; set; }
    }
    
   

    public class Work : _Model
    {
        [Required]
        public string Profession { get; set; }
        public string CompanyTitle { get; set; }
        public string DirectManager { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string PostalBox { get; set; }
        public string PostalCode { get; set; }
    }
    

   

    public class IsslamRecognition:_Model
    {
        public string Title { get; set; }
        public List<Muslime> muslimes { get; set; }
    }
    

    
    
    
   

    public class Attachment:_Model
    {
        public ImageType ImageType { get; set; }
        [NotMapped]
        public IFormFile AttachmentFile { get; set; }
        public byte[] AttachmentValue  { get; set; }
        public string AttachmentBase64 { get; set; }
    }
    
  
 
     
}