namespace InnoClinic.Notification.Core.Models.NotificationModels;

/// <summary>
/// Represents a request to send an appointment result document via email.
/// </summary>
public record SendAppointmentResultDocumentRequest(
    /// <summary>
    /// Gets the byte array representing the PDF document to be sent.
    /// </summary>
    byte[] PdfBytes,

    /// <summary>
    /// Gets the email address to which the appointment result document will be sent.
    /// </summary>
    string Email
);