namespace Moia.Shared.ViewModels
{
    public class GenericResult<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }

}
