namespace Moia.Shared.ModelInterfaces
{
    public interface IAuditableInsert
    {
        int? CreatedBy { get; set; }
        DateTimeOffset? CreatedOn { get; set; }
    }

}
