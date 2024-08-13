using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class AttachmentDto : _Model
    {
        public int MuslimeId { get; set; }
        public IFormFile Personal { get; set; }
        public IFormFile Accomodation { get; set; }
        public IFormFile Passport { get; set; }

        [NotMapped]
        public string _Personal { get; set; }
        [NotMapped]
        public string _Accomodation { get; set; }
        [NotMapped]
        public string _Passport { get; set; }
    }

}
    