namespace Moia.Shared.Models
{
    public class OrderHistory : _Model
    {
        public DateTime ActionDate { get; set; }
        public string Description { get; set; }
        public OrderStatus OrderStatus { get; set; }
 
        public virtual Order Order { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
    }
    
   

    
} 
