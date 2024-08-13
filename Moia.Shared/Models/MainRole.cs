namespace Moia.Shared.Models
{
    public class MainRole :_Model
    {
        [Required]
        [MaxLength(100)]
        public string Code { get; set; }
        public string NameAr { get; set; }
        public virtual ICollection<MainUserRole> MainUserRole { get; set; }
    }
    
    public class MainRoleNameId
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
    }

}