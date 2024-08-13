namespace Moia.Shared.ViewModels
{
    public class ErrorLogViewModel : LogViewModel
    {
        public string Id { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
