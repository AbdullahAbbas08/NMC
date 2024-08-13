using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class OriginalCountryDto : _Model
    {
        public int MuslimeId { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
        public string DoorNumber { get; set; }
    }


}
