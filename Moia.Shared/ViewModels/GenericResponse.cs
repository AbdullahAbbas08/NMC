namespace Moia.Shared.ViewModels
{
    public class GenericResponse<T>  where T : class
    {
        public GenericResponse(T value)
        {
            Data = value;
        }
        public T Data { get; set; }
    }

}
