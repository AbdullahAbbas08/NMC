using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class CurrentResidenceDto : _Model
    {
        public int MuslimeId { get; set; }
        [Required]
        public string City { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string DoorNumber { get; set; }
        public string EmergencyNumber { get; set; }
    }

}
