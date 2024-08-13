using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class FamilyInformationDto : _Model
    {
        public int MuslimeId { get; set; }
        public int MembersNumber { get; set; }
        public int BoysNumber { get; set; }
        public int GirlsNumber { get; set; }
    }


}
