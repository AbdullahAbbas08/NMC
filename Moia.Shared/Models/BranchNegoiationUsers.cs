namespace Moia.Shared.Models
{
    public class BranchNegoiationUsers:_Model
    {

        public int BranchId { get; set; }
        public int UserId { get; set; }

        [ForeignKey("BranchId")]
        public virtual MinistryBransh Bransh { get; set; }

        [ForeignKey("UserId")]
        public virtual MainUser User { get; set; }
    }
}
