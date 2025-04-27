using InnoClinic.Notification.Application.Services;
using InnoClinic.Notification.Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace InnoClinic.Notification.Application;

/// <summary>
/// Contains extension methods for adding services and FluentValidation to the service collection in the InnoClinic Profiles application.
/// </summary>
public static class InnoClinicNotificationApplicationInjection
{
    /// <summary>
    /// Adds services related to RabbitMQ, service category management, specialization management and medical service management to the service collection.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/> with added services.</returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
