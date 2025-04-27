using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using InnoClinic.Notification.Core.Models.NotificationModels;
using InnoClinic.Notification.Core.Abstractions;

namespace InnoClinic.Notification.API.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    [HttpPost("send-verification-email")]
    public async Task<ActionResult> SendVerificationEmailAsync(
        [FromBody] SendVerificationEmailRequest sendVerificationEmailRequest)
    {
        await _notificationService.SendVerificationEmailAsync(sendVerificationEmailRequest);

        return Ok();
    }

    [HttpPost("send-login-information-email")]
    public async Task<ActionResult> SendLoginInformationEmailAsync(
        [FromBody] SendLoginInformationEmailRequest sendLoginInformationEmailRequest)
    {
        await _notificationService.SendLoginInformationEmailAsync(sendLoginInformationEmailRequest);

        return Ok();
    }

    [HttpPost("send-notification-about-appointment")]
    public async Task<ActionResult> SendNotificationAboutAppointmentAsync(
        [FromBody] SendNotificationAboutAppointmentRequest sendNotificationAboutAppointmentRequest)
    {
        await _notificationService.SendNotificationAboutAppointmentAsync(sendNotificationAboutAppointmentRequest);

        return Ok();
    }

    [HttpPost("send-appointment-result-document")]
    public async Task<ActionResult> SendAppointmentResultDocumentAsync(
        [FromBody] SendAppointmentResultDocumentRequest sendNotificationAboutAppointmentRequest)
    {
        await _notificationService.SendAppointmentResultDocumentAsync(sendNotificationAboutAppointmentRequest);

        return Ok();
    }
}