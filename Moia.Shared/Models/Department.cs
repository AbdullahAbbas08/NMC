namespace Moia.Shared.Models
{
    public class Department: _Model
    {
        public string Title { get; set; }

        public int BranshID  { get; set; } 

        [ForeignKey("BranshID")]
        public virtual MinistryBransh MinistryBransh { get; set; }

        public virtual MainUserRole MangerRole { get; set; }

    }




}
