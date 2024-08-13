namespace Moia.Shared.ViewModels
{
    public class LoginResult
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public string refreshToken { get; set; }
        public string count { get; set; }
        public bool Passchanged { get; set; }
        public bool IsActiveDirectoy { get; set; }
    }

}
 