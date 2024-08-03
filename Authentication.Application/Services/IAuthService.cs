namespace Authentication.Application.Services
{
    public interface IAuthService
    {
        Task<string?> GetToken(string email, string password);
    }
}
