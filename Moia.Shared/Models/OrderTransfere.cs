namespace Moia.Shared.Models
{
    public class OrderTransfere
    {
        public int ID { get; set; }

        public int FromUserId { get; set; }
        public int ToUserId { get; set; }

        [ForeignKey("FromUserId")]
        public virtual MainUser FromUser { get; set; }

        [ForeignKey("ToUserId")]
        public virtual MainUser ToUser { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
    
   

}