namespace InnoClinic.Notification.Infrastructure.Options.Email;

/// <summary>
/// Represents the options for configuring email settings.
/// </summary>
public class EmailOptions
{
    /// <summary>
    /// Gets or sets the email address to be used for sending emails.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the application password for the email account.
    /// This is used for authentication when sending emails.
    /// </summary>
    public string EmailAppPassword { get; set; } = string.Empty;
}