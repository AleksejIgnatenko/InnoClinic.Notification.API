using Microsoft.AspNetCore.Mvc;

namespace InnoClinic.Notification.Application.Services;

public interface INotificationService
{
    /// <summary>
    /// Asynchronously sends a verification email to the specified account.
    /// </summary>
    /// <param name="account">The account model containing user information.</param>
    /// <param name="urlHelper">The URL helper to generate confirmation links.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendVerificationEmailAsync(Guid accountId, string email, string callbackUrl);

    /// <summary>
    /// Sends a notification about an appointment via email.
    /// </summary>
    /// <param name="accountId">The unique identifier of the account.</param>
    /// <param name="patientFullName">The full name of the patient.</param>
    /// <param name="date">The date of the appointment.</param>
    /// <param name="time">The time of the appointment.</param>
    /// <param name="medicalServiceName">The name of the medical service.</param>
    /// <param name="doctorFullName">The full name of the doctor.</param>
    Task SendNotificationAboutAppointmentAsync(Guid accountId, string patientFullName, string date,
        string time, string medicalServiceName, string doctorFullName);
}