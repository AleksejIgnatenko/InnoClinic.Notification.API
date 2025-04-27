using Microsoft.AspNetCore.DataProtection;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using InnoClinic.Notification.Core.Abstractions;
using InnoClinic.Notification.Infrastructure.Options.Email;
using InnoClinic.Notification.Core.Models.NotificationModels;

namespace InnoClinic.Notification.Application.Services;

/// <summary>
/// Provides methods for sending various email notifications related to user accounts and appointments.
/// </summary>
public class NotificationService(IOptions<EmailOptions> emailSetting, IDataProtectionProvider dataProtectionProvider, IEncryptionService encryptionService) : INotificationService
{
    private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("EmailConfirmation");
    private readonly HttpClient _httpClient = new();
    private readonly IEncryptionService _encryptionService = encryptionService;
    private readonly EmailOptions _emailSetting = emailSetting.Value;

    /// <summary>
    /// Asynchronously sends a verification email to the specified email address.
    /// </summary>
    /// <param name="sendVerificationEmailRequest">The request containing the email address and callback URL for verification.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendVerificationEmailAsync(SendVerificationEmailRequest sendVerificationEmailRequest)
    {
        var token = GenerateEmailConfirmationToken(sendVerificationEmailRequest.Email);

        var subject = "Подтвердите ваш аккаунт";
        string message = $@"
                <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 0 auto;
                                background-color: #ffffff;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            h1 {{
                                color: #333333;
                            }}
                            p {{
                                color: #555555;
                                line-height: 1.6;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                margin-top: 20px;
                                background-color: #007BFF;
                                color: #ffffff !important; 
                                text-decoration: none;
                                border-radius: 5px;
                                font-weight: bold;
                            }}
                            .button:hover {{
                                background-color: #0056b3; 
                            }}
                            .footer {{
                                margin-top: 20px;
                                text-align: center;
                                color: #999999;
                                font-size: 12px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h1>Подтвердите ваш Email</h1>
                            <p>Здравствуйте!</p>
                            <p>Благодарим вас за регистрацию на нашем сайте. Для завершения процесса подтверждения вашего адреса электронной почты, пожалуйста, перейдите по ссылке ниже:</p>
                            <p><a href='{sendVerificationEmailRequest.CallbackUrl}' class='button'>Подтвердить аккаунт</a></p>
                            <p>Если вы не регистрировались на нашем сайте, просто проигнорируйте это письмо.</p>
                            <div class='footer'>
                                С уважением,<br>
                                Администрация сайта InnoClinic
                            </div>
                        </div>
                    </body>
                </html>";

        await SendEmailAsync(sendVerificationEmailRequest.Email, subject, message);
    }

    /// <summary>
    /// Asynchronously sends login information to the specified email address.
    /// </summary>
    /// <param name="sendLoginInformationEmailRequest">The request containing the user's email address, encrypted password, and full name.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendLoginInformationEmailAsync(SendLoginInformationEmailRequest sendLoginInformationEmailRequest)
    {
        var password = _encryptionService.DecryptData(sendLoginInformationEmailRequest.EncryptedPassword);

        var subject = "Данные для входа";
        string message = $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                        }}
                        h1 {{
                            color: #333333;
                        }}
                        p {{
                            color: #555555;
                            line-height: 1.6;
                        }}
                        .button {{
                            display: inline-block;
                            padding: 10px 20px;
                            margin-top: 20px;
                            background-color: #007BFF;
                            color: #ffffff !important;
                            text-decoration: none;
                            border-radius: 5px;
                            font-weight: bold; 
                        }}
                        .button:hover {{
                            background-color: #0056b3; 
                        }}
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            color: #999999;
                            font-size: 12px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Добро пожаловать в InnoClinic, {sendLoginInformationEmailRequest.FullName}!</h1>
                        <p>Здравствуйте!</p>
                        <p>Мы рады сообщить вам, что вы успешно зарегистрированы в системе InnoClinic. Теперь вы можете управлять своей практикой и взаимодействовать с пациентами.</p>
                        <p>Ваши учетные данные для входа:</p>
                        <p><strong>Логин:</strong> {sendLoginInformationEmailRequest.Email}</p>
                        <p><strong>Пароль:</strong> {password}</p>
                        <p>Для доступа к вашей учетной записи, пожалуйста, перейдите по следующей ссылке:</p>
                        <p><a href=""http://localhost:4001"" class='button'>Войти в систему</a></p>
                        <div class='footer'>
                            С уважением,<br>
                            Администрация сайта InnoClinic
                        </div>
                    </div>
                </body>
            </html>";

        await SendEmailAsync(sendLoginInformationEmailRequest.Email, subject, message);
    }

    /// <summary>
    /// Asynchronously sends a notification about an appointment to the patient via email.
    /// </summary>
    /// <param name="sendNotificationAboutAppointmentRequest">The request containing account ID, patient details, appointment date and time, medical service name, and doctor's full name.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendNotificationAboutAppointmentAsync(SendNotificationAboutAppointmentRequest sendNotificationAboutAppointmentRequest)
    {
        var response = await _httpClient.GetAsync($"http://innoclinic_authorization_api:8080/api/Account/email-by-account-id/{sendNotificationAboutAppointmentRequest.AccountId}");
        var email = await response.Content.ReadAsStringAsync();

        var subject = "Напоминание о записи на прием";
        string message = $@"
                <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 0 auto;
                                background-color: #ffffff;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            h1 {{
                                color: #333333;
                            }}
                            p {{
                                color: #555555;
                                line-height: 1.6;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                margin-top: 20px;
                                background-color: #007BFF;
                                color: #ffffff !important; 
                                text-decoration: none;
                                border-radius: 5px;
                                font-weight: bold;
                            }}
                            .button:hover {{
                                background-color: #0056b3; 
                            }}
                            .footer {{
                                margin-top: 20px;
                                text-align: center;
                                color: #999999;
                                font-size: 12px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <p>Здравствуйте {sendNotificationAboutAppointmentRequest.PatientFullName}!</p>
                            <p>У вас имеется запись на {sendNotificationAboutAppointmentRequest.Date} в {sendNotificationAboutAppointmentRequest.Time}.</p>
                            <p>На {sendNotificationAboutAppointmentRequest.MedicalServiceName}.</p>
                            <p>К {sendNotificationAboutAppointmentRequest.DoctorFullName}.</p>
                            <div class='footer'>
                                С уважением,<br>
                                Администрация сайта InnoClinic
                            </div>
                        </div>
                    </body>
                </html>";

        await SendEmailAsync(email, subject, message);
    }

    /// <summary>
    /// Asynchronously sends the appointment result document to the specified email address.
    /// </summary>
    /// <param name="sendAppointmentResultDocumentRequest">The request containing the recipient's email address and the PDF document bytes.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SendAppointmentResultDocumentAsync(SendAppointmentResultDocumentRequest sendAppointmentResultDocumentRequest)
    {
        var subject = "результаты приемы";
        string message = $@"
                <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 0;
                            }}
                            .container {{
                                max-width: 600px;
                                margin: 0 auto;
                                background-color: #ffffff;
                                padding: 20px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            h1 {{
                                color: #333333;
                            }}
                            p {{
                                color: #555555;
                                line-height: 1.6;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                margin-top: 20px;
                                background-color: #007BFF;
                                color: #ffffff !important; 
                                text-decoration: none;
                                border-radius: 5px;
                                font-weight: bold;
                            }}
                            .button:hover {{
                                background-color: #0056b3; 
                            }}
                            .footer {{
                                margin-top: 20px;
                                text-align: center;
                                color: #999999;
                                font-size: 12px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <p>Результаты приема!</p>
                            <div class='footer'>
                                С уважением,<br>
                                Администрация сайта InnoClinic
                            </div>
                        </div>
                    </body>
                </html>";

        await SendEmailAsync(sendAppointmentResultDocumentRequest.Email, subject, message, sendAppointmentResultDocumentRequest.PdfBytes);
    }

    private string GenerateEmailConfirmationToken(string email)
    {
        var protectedToken = _dataProtector.Protect(email);
        return protectedToken;
    }

    private async Task SendEmailAsync(string email, string subject, string message, byte[] pdfBytes = null)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Inno Clinic", _emailSetting.Email));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message
        };

        if (pdfBytes != null)
        {
            var pdfAttachment = new MimePart("application", "pdf")
            {
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Default,
                FileName = "AppointmentResult.pdf",
                Content = new MimeContent(new MemoryStream(pdfBytes))
            };
            bodyBuilder.Attachments.Add(pdfAttachment);
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_emailSetting.Email, _emailSetting.EmailAppPassword);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);
    }
}