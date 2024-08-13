namespace Moia.Shared.Models
{
    public class DepartmentNegoiationUsers:_Model
    {

        public int DepartmentId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Department Department { get; set; }

        [ForeignKey("UserId")]
        public virtual MainUser User { get; set; }
    }
}
