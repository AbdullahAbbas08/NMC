namespace Moia.Shared.ModelInterfaces
{
    public interface IAssertableConcurrencyStamp
    {
        string ConcurrencyStamp { get; set; }
    }

}
