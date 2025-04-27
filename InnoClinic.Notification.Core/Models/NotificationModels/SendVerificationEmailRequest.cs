namespace InnoClinic.Notification.Core.Models.NotificationModels;

/// <summary>
/// Represents a request to send a verification email.
/// </summary>
public record SendVerificationEmailRequest(
    /// <summary>
    /// Gets the unique identifier of the account associated with the email.
    /// </summary>
    Guid AccountId,

    /// <summary>
    /// Gets the email address to which the verification email will be sent.
    /// </summary>
    string Email,

    /// <summary>
    /// Gets the URL to which the user will be redirected after verification.
    /// </summary>
    string CallbackUrl
);
