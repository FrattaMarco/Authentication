using ClientUsers;

namespace Authentication.Application.NSwag
{
    public interface IUsersClientFactory
    {
        UsersClient Create();
    }
}
