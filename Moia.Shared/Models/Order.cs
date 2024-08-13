namespace Moia.Shared.Models
{
    public class Order : _Model
    {
        public string Code { get; set; }
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual MainUser DataEntry { get; set; }
        public virtual Committee Committee { get; set; }
        public virtual Muslime Muslime { get; set; }
        public OrderStage Stage { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
