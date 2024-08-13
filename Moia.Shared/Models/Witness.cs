namespace Moia.Shared.Models
{
    public class Witness : _Model
    {
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Mobile { get; set; }
        public virtual ICollection<PersonalData> PersonalDatas { get; set; }
    }
    
    
}
 