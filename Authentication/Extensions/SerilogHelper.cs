using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace Authentication.Extensions
{
    public static class SerilogHelper
    {
        public static void ConfigureLogging(IConfiguration confs)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new ArgumentNullException(nameof(confs));
            IConfigurationRoot configurationRoot = (IConfigurationRoot)confs;

            //Definisco il template dei log
            string template = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {CorrelationId} {Environment} {NewLine}{Exception}";

            var loggerConfig = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .Enrich.WithExceptionDetails()
               .WriteTo.Console(outputTemplate: template)
               .Enrich.WithProperty("Environment", environment)
               .ReadFrom.Configuration(configurationRoot);

            if (environment == "Docker")
            {
                loggerConfig.WriteTo.Elasticsearch(ConfigureElasticSearchSink(configurationRoot, environment));
            }
            else if (environment == "Development")
            {
                loggerConfig.WriteTo.File($"../Logs-Authentication/Authentication-{DateTime.Now:yyyy-MM-dd}.txt",               
                outputTemplate: template);
            }

            Log.Logger = loggerConfig.CreateLogger();
        }
        private static ElasticsearchSinkOptions ConfigureElasticSearchSink(IConfigurationRoot configuration, string enviornment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration.GetSection("ElasticConfiguration:Uri").Value!))
            {
                ModifyConnectionSettings = x => x.BasicAuthentication(configuration.GetSection("ElasticConfiguration:ElkUsername").Value, configuration.GetSection("ElasticConfiguration:ElkPassword").Value),
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly()!.GetName()!.Name!.ToLower().Replace(".", "-")}-{enviornment.ToLower()}",
                NumberOfReplicas = 1,
                NumberOfShards = 2               
            };
        }
    }
}
