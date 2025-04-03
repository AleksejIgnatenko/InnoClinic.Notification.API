namespace InnoClinic.Notification.Core.Models.NotificationModels
{
    public record SendVerificationEmailRequest (
        Guid AccountId,
        string Email,
        string CallbackUrl);
}
