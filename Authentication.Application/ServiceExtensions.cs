using Authentication.Application.Configurations;
using Authentication.Application.NSwag;
using Authentication.Application.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Mime;
using System.Text.Json;

namespace Authentication.Application
{
    public static class ServiceExtensions
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration confs)
        {
            services.AddHttpClient();
            #region HealthCheck
            services.AddHealthChecks().AddCheck("Api.Authentication Health Check", () => HealthCheckResult.Healthy("Api.Authentication is healthy"))
                                      .AddUrlGroup(new Uri(confs.GetSection("ApiBaseUrlUsers").Value + "/health"), "Api.Users Health Check");

            services.Configure<HealthCheckOptions>(options =>
            {
                options.ResponseWriter = async (context, report) =>
                {
                    var result = new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Key.Replace("Health Check", "")
                        })
                    };
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(result));
                };
            });
            #endregion           
            services.AddSingleton<IUsersClientFactory, UsersClientFactory>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceExtensions).Assembly));
            services.Configure<TokenConfigs>(confs.GetSection("TokenConfigs"));
        }
    }
}
