namespace Moia.Shared.ViewModels
{
    public class SMSTemplate 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SMSTemplateId { get; set; }
        public string SMScode { get; set; }
        public string TemplateCode { get; set; }
        public string Parameters { get; set; }
        public string TextMessage { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
    }
}
