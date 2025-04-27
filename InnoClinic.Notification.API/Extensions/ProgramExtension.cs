using InnoClinic.Notification.API.Middlewares;
using InnoClinic.Notification.Application;
using InnoClinic.Notification.Infrastructure.Options.Email;
using InnoClinic.Notification.Infrastructure.Options.Jwt;
using Microsoft.Extensions.Options;
using Serilog;

namespace InnoClinic.Notification.API.Extensions;

/// <summary>
/// Contains extension methods for configuring the web application builder and application startup.
/// </summary>
public static class ProgramExtension
{
    /// <summary>
    /// Configures the web application builder with necessary services and configurations.
    /// </summary>
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .CreateSerilog(builder.Host);

        builder.Configuration
            .AddEnvironmentVariables()
            .LoadConfiguration();

        builder.Services
            .AddOptions(builder.Configuration)
            .AddServices()
            .AddSwaggerGen()
            .AddEndpointsApiExplorer()
            .AddJwtAuthentication(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>())
            .AddHttpClient()
            .AddHttpContextAccessor()
            .AddDataProtection();

        builder.Services
            .AddControllers();

        return builder;
    }

    /// <summary>
    /// Configures the web application with necessary middleware and services during startup.
    /// </summary>
    public static WebApplication ConfigureApplicationAsync(this WebApplication app)
    {
        app.UseCustomExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    private static IConfiguration LoadConfiguration(this IConfigurationBuilder configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        return configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("emailsetting.json", optional: true, reloadOnChange: true)
            .Build();
    }

    private static IServiceCollection AddOptions(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

        return services;
    }

    private static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder webApplication)
    {
        webApplication.UseMiddleware<ExceptionHandlerMiddleware>();

        return webApplication;
    }
}
