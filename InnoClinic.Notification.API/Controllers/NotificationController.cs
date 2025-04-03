using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using InnoClinic.Notification.Application.Services;
using InnoClinic.Notification.Core.Models.NotificationModels;

namespace InnoClinic.Notification.API.Controllers;

[ExcludeFromCodeCoverage]
[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpPost("send-verification-email")]
    public async Task<ActionResult> SendVerificationEmailAsync(
        [FromBody] SendVerificationEmailRequest sendVerificationEmailRequest)
    {
        Console.WriteLine("11111111111111111111");
        await _notificationService.SendVerificationEmailAsync(sendVerificationEmailRequest.AccountId, sendVerificationEmailRequest.Email, sendVerificationEmailRequest.CallbackUrl);

        return Ok();
    }

    [HttpPost("send-notification-about-appointment")]
    public async Task<ActionResult> SendNotificationAboutAppointmentAsync(
        [FromBody] SendNotificationAboutAppointmentRequest sendNotificationAboutAppointmentRequest)
    {
        await _notificationService.SendNotificationAboutAppointmentAsync(sendNotificationAboutAppointmentRequest.AccountId,
            sendNotificationAboutAppointmentRequest.PatientFullName, sendNotificationAboutAppointmentRequest.Date,
            sendNotificationAboutAppointmentRequest.Time,
            sendNotificationAboutAppointmentRequest.MedicalServiceName,
            sendNotificationAboutAppointmentRequest.DoctorFullName);

        return Ok();
    }
}