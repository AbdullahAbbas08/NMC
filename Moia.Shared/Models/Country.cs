namespace Moia.Shared.Models
{
    public class Country:_Model
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
    }

    public class countryFromFile
    {
        public string code { get; set; }
        public string arabic_name { get; set; }
        public string english_name { get; set; }
        public string dialCode { get; set; }
    }
    
    public class CitiesFromFile
    {
        public List<string> cities { get; set; }
    }
     

}