namespace Moia.Shared.Models
{
    public class Localization : _Model
    {
        [MaxLength(100)]
        [Required]
        public string Key { get; set; }
        [Required]
        public string ValueAr { get; set; }
        public string ValueEn { get; set; }
    }




}
