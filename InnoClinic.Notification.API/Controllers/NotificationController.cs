using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using InnoClinic.Notification.Core.Models.NotificationModels;
using InnoClinic.Notification.Core.Abstractions;

namespace InnoClinic.Notification.API.Controllers;

/// <summary>
/// Controller for managing notifications.
/// </summary>
[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    /// <summary>
    /// Send a verification email asynchronously.
    /// </summary>
    /// <param name="sendVerificationEmailRequest">Request to send a verification email.</param>
    /// <returns>ActionResult indicating the completion of the operation.</returns>
    [HttpPost("send-verification-email")]
    public async Task<ActionResult> SendVerificationEmailAsync(
        [FromBody] SendVerificationEmailRequest sendVerificationEmailRequest)
    {
        await _notificationService.SendVerificationEmailAsync(sendVerificationEmailRequest);

        return Ok();
    }

    /// <summary>
    /// Send login information email asynchronously.
    /// </summary>
    /// <param name="sendLoginInformationEmailRequest">Request to send login information email.</param>
    /// <returns>ActionResult indicating the completion of the operation.</returns>
    [HttpPost("send-login-information-email")]
    public async Task<ActionResult> SendLoginInformationEmailAsync(
        [FromBody] SendLoginInformationEmailRequest sendLoginInformationEmailRequest)
    {
        await _notificationService.SendLoginInformationEmailAsync(sendLoginInformationEmailRequest);

        return Ok();
    }

    /// <summary>
    /// Send notification about an appointment asynchronously.
    /// </summary>
    /// <param name="sendNotificationAboutAppointmentRequest">Request to send notification about an appointment.</param>
    /// <returns>ActionResult indicating the completion of the operation.</returns>
    [HttpPost("send-notification-about-appointment")]
    public async Task<ActionResult> SendNotificationAboutAppointmentAsync(
        [FromBody] SendNotificationAboutAppointmentRequest sendNotificationAboutAppointmentRequest)
    {
        await _notificationService.SendNotificationAboutAppointmentAsync(sendNotificationAboutAppointmentRequest);

        return Ok();
    }

    /// <summary>
    /// Send appointment result document asynchronously.
    /// </summary>
    /// <param name="sendAppointmentResultDocumentRequest">Request to send appointment result document.</param>
    /// <returns>ActionResult indicating the completion of the operation.</returns>
    [HttpPost("send-appointment-result-document")]
    public async Task<ActionResult> SendAppointmentResultDocumentAsync(
        [FromBody] SendAppointmentResultDocumentRequest sendAppointmentResultDocumentRequest)
    {
        await _notificationService.SendAppointmentResultDocumentAsync(sendAppointmentResultDocumentRequest);

        return Ok();
    }
}