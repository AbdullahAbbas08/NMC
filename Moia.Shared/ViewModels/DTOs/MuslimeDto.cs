using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class MuslimeDto : _Model
    {
        public virtual PersonalDataDto PersonalData { get; set; }
        public virtual PersonalInformationDto PersonalInformation { get; set; }
        public virtual OriginalCountryDto OriginalCountry { get; set; }
        public virtual CurrentResidenceDto CurrentResidence { get; set; }
        public virtual ContactAndInfoDataViewModel ContactData { get; set; }
        public virtual FamilyAndWorkDto FamilyInformation { get; set; }
        public virtual IslamRecognitionWayDto IsslamRecognition { get; set; }
        public virtual AttachmentDto Attachment { get; set; }
        public virtual List<WitnessDto> Witness { get; set; }
    }
}
