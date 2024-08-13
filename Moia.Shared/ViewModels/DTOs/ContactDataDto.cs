using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class ContactDataDto : _Model
    {
        public int MuslimeId { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string HomeNumber { get; set; }
        public string WorkNumber { get; set; }
        public string Email { get; set; }
    }

}
