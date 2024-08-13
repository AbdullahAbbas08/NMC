namespace Moia.Shared.ViewModels
{
    public class ManageCardDto
    {
        public ActionType actionType { get; set; }
        public string BearerToaken { get; set; }
        public DateTime expirationDate { get; set; }
        public string referenceNo { get; set; }
        public long uniqueCardId { get; set; }
        public long documentNo { get; set; }
        public string language { get; set; } 
        public List<cardAttributes> cardAttributes { get; set; }
        public List<cardAttributes> backCardAttributes { get; set; }
    }

    public enum ActionType
    {
        AddCard = 1,
        UpdateCard,
        DeleteCard
    }

}
