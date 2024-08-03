using Authentication.Application.Repositories;
using Authentication.Persistence.Repositories;
using DapperContext.Application.Repositories;
using DapperContext.Persistence.Context;
using DapperContext.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authentication.Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration conf)
        {
            string connectionString = conf.GetConnectionString("ConnectionStringUsers") ?? throw new ArgumentNullException(nameof(conf));
            services.AddScoped<ContextDapper>(x => new(connectionString));
            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        }
    }
}
