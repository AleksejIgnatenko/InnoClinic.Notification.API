using InnoClinic.Notification.Core.Models.NotificationModels;

namespace InnoClinic.Notification.Core.Abstractions;

/// <summary>
/// Defines methods for sending various notifications via email.
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// Asynchronously sends a verification email to the specified account.
    /// </summary>
    /// <param name="sendVerificationEmailRequest">The request containing account ID, email address, and callback URL.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendVerificationEmailAsync(SendVerificationEmailRequest sendVerificationEmailRequest);

    /// <summary>
    /// Asynchronously sends login information to the specified email address.
    /// </summary>
    /// <param name="sendLoginInformationEmailRequest">The request containing email address, encrypted password, and full name.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendLoginInformationEmailAsync(SendLoginInformationEmailRequest sendLoginInformationEmailRequest);

    /// <summary>
    /// Asynchronously sends a notification about an appointment via email.
    /// </summary>
    /// <param name="sendNotificationAboutAppointmentRequest">The request containing account ID, patient details, appointment date and time, medical service name, and doctor’s full name.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendNotificationAboutAppointmentAsync(SendNotificationAboutAppointmentRequest sendNotificationAboutAppointmentRequest);

    /// <summary>
    /// Asynchronously sends an appointment result document to the specified email.
    /// </summary>
    /// <param name="sendAppointmentResultDocumentRequest">The request containing the PDF document bytes and the recipient email address.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendAppointmentResultDocumentAsync(SendAppointmentResultDocumentRequest sendAppointmentResultDocumentRequest);
}