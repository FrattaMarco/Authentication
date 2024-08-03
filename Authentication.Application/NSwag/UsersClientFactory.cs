using ClientUsers;
using Microsoft.Extensions.Configuration;

namespace Authentication.Application.NSwag
{
    public class UsersClientFactory(IConfiguration config, IHttpClientFactory httpClientFactory) : IUsersClientFactory
    {
        private readonly IConfiguration _config = config ?? throw new ArgumentNullException(nameof(config));
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        public UsersClient Create()
        {
            var apiClient = new UsersClient(_config["ApiBaseUrlUsers"], _httpClientFactory.CreateClient());
            return apiClient;
        }
    }
}
