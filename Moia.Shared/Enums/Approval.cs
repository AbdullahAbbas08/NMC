namespace Moia.DoL.Enums
{
    public enum OrderStatus
    {
        Create,
        Send,
        Accept ,
        Reject,
        Finished,
        Printed
    }
    
    public enum OrderStage
    {
       DataEntry,
       Committee,
       Department,
       Branch,
       ReadyToPrintCard,
       Other
    }

} 
  