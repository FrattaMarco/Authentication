using Authentication.Application.Model;
using Authentication.Application.Repositories;
using DapperContext.Application.Repositories;
using System.Data;

namespace Authentication.Persistence.Repositories
{
    public class AuthenticationRepository(IGenericRepository genericRepository) : IAuthenticationRepository
    {
        private readonly IGenericRepository _genericRepository = genericRepository;

        public async Task<UserModel?> GetUtente(string email)
        {
            IEnumerable<UserModel> users = await _genericRepository.QueryGetAsyncByStoredProcedure<UserModel>("GetUserByParameters", new { Email = email }, CommandType.StoredProcedure);
            return users.SingleOrDefault();
        }
    }
}