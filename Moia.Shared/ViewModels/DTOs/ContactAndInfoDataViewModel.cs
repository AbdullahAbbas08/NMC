using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class ContactAndInfoDataViewModel
    {
        public int MuslimeId { get; set; }
        public OriginalCountry OriginalCountry { get; set; }
        public CurrentResidence CurrentResidence { get; set; }
        public ContactData ContactData { get; set; }
    }

}
 