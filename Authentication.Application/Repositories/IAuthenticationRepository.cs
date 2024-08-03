using Authentication.Application.Model;

namespace Authentication.Application.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<UserModel?> GetUtente(string email);
    }
}
