using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class PersonalInformationDto : _Model
    {
        public int MuslimeId { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public DateTime DateOfEntryKingdom { get; set; }

        [Required]
        public string PlaceOfBirth { get; set; }

        [Required]
        public Country Nationality { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public Religion PreviousReligion { get; set; }

        public int? PositionInFamily { get; set; }

        public MaritalStatus? MaritalStatus { get; set; }

        public string? HusbandName { get; set; }

        [Required]
        public EducationalLevel EducationalLevel { get; set; }

        //[Required]
        public string ResidenceNumber { get; set; }

        [Required]
        public DateTime ResidenceIssueDate { get; set; }

        [Required]
        public ResidenceIssuePlace ResidenceIssuePlace { get; set; }

        //[Required]
        public string PassportNumber { get; set; }

        [Required]
        public DateTime DateOfPassportIssue { get; set; }
        //[Required]
        public string PlaceOfPassportIssue { get; set; }
    }

   
}
