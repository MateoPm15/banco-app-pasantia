public interface IAuthService
{
    string? Authenticate(string email, string password);
}
