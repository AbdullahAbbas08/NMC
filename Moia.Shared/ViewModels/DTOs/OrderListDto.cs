namespace Moia.Shared.ViewModels.DTOs
{
    public class OrderListDto
    {
        public string OrderCode { get; set; }
        public string? MuslimeName { get; set; }
        public int MuslimeId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool isChecked { get; set; }
        public string Stage { get; set; }
        public List<OrderHistoryDto> OrderTimeLine { get; set; }
    }

}
     