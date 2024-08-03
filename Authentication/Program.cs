using Authentication.Application;
using Authentication.Application.Filter;
using Authentication.Extensions;
using Authentication.Middleware;
using Authentication.Persistence;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

SerilogHelper.ConfigureLogging(builder.Configuration);

// Add services to the container.
builder.Services.ConfigureApplication(builder.Configuration);
builder.Services.ConfigurePersistence(builder.Configuration);
builder.Services.AddControllers();
builder.Services.ConfigureSwagger();

if (builder.Environment.EnvironmentName == "Docker")
{
    string certificateName = Environment.GetEnvironmentVariable("CertName")!;
    string certificatePassword = Environment.GetEnvironmentVariable("CertPassword")!;
    builder.WebHost.UseUrls("https://*:444");
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.ConfigureHttpsDefaults(listenOptions =>
        {
            listenOptions.ServerCertificate = new X509Certificate2($"{certificateName}.pfx", certificatePassword);
        });
    });
}

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api.Authentication", Version = "v1" });
});

builder.Host.UseSerilog();
builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add("X-Correlation-ID");
});

var app = builder.Build();
app.UseExceptionHandler();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseHeaderPropagation();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api.Authentication v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();