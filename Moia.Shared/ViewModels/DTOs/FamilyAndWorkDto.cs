using Moia.Shared.Models;

namespace Moia.Shared.ViewModels.DTOs
{
    public class FamilyAndWorkDto
    {
        public int MuslimeId { get; set; }
        public FamilyInformation FamilyInformation { get; set; }
        public Work Work { get; set; }
    }

}
