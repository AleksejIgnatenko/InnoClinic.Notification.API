namespace InnoClinic.Notification.Core.Models.NotificationModels;

public record SendNotificationAboutAppointmentRequest(
    /// <summary>
    /// The unique identifier of the account.
    /// </summary>
    Guid AccountId,
    /// <summary>
    /// The full name of the patient.
    /// </summary>
    string PatientFullName,
    /// <summary>
    /// The date of the appointment.
    /// </summary>
    string Date,
    /// <summary>
    /// The time of the appointment.
    /// </summary>
    string Time,
    /// <summary>
    /// The name of the medical service.
    /// </summary>
    string MedicalServiceName,
    /// <summary>
    /// The full name of the doctor.
    /// </summary>
    string DoctorFullName
);