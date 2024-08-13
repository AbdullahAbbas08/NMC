using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class WorkDto : _Model
    {
        public int MuslimeId { get; set; }
        [Required]
        public string Profession { get; set; }
        public string CompanyTitle { get; set; }
        public int DirectManager { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Address { get; set; }
        public string PostalBox { get; set; }
        public string PostalCode { get; set; }
    }

}
