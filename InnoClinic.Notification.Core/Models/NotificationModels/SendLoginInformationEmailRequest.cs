namespace InnoClinic.Notification.Core.Models.NotificationModels;

/// <summary>
/// Represents a request to send login information via email.
/// </summary>
public record SendLoginInformationEmailRequest(
    /// <summary>
    /// Gets the email address of the user to whom the login information will be sent.
    /// </summary>
    string Email,

    /// <summary>
    /// Gets the encrypted password for the user.
    /// </summary>
    string EncryptedPassword,

    /// <summary>
    /// Gets the full name of the user.
    /// </summary>
    string FullName
);