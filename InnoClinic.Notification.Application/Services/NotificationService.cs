using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MimeKit;

namespace InnoClinic.Notification.Application.Services
{
    /// <summary>
    /// Provides email services, including sending verification emails and confirming email addresses.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private const string _email = "innoclinic33@gmail.com";
        private const string _emailAppPassword = "spaz tebr scpd lahu";

        private readonly IDataProtector _dataProtector;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="dataProtectionProvider">The data protection provider for token generation.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor for generating URLs.</param>
        public NotificationService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("EmailConfirmation");

            _httpClient = new HttpClient();
        }

        /// <summary>
        /// Asynchronously sends a verification email to the specified account.
        /// </summary>
        /// <param name="account">The account model containing user information.</param>
        /// <param name="urlHelper">The URL helper to generate confirmation links.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendVerificationEmailAsync(Guid accountId, string email, string callbackUrl)
        {
            var token = GenerateEmailConfirmationToken(email);

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
                            <p><a href='{callbackUrl}' class='button'>Подтвердить аккаунт</a></p>
                            <p>Если вы не регистрировались на нашем сайте, просто проигнорируйте это письмо.</p>
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
        /// Sends a notification about an appointment via email.
        /// </summary>
        /// <param name="accountId">The unique identifier of the account.</param>
        /// <param name="patientFullName">The full name of the patient.</param>
        /// <param name="date">The date of the appointment.</param>
        /// <param name="time">The time of the appointment.</param>
        /// <param name="medicalServiceName">The name of the medical service.</param>
        /// <param name="doctorFullName">The full name of the doctor.</param>
        public async Task SendNotificationAboutAppointmentAsync(Guid accountId, string patientFullName, string date,
            string time, string medicalServiceName, string doctorFullName)
        {
            var response = await _httpClient.GetAsync($"http://innoclinic_notification_api:8080/api/Account/email-by-account-id/{accountId}");
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
                            <p>Здравствуйте {patientFullName}!</p>
                            <p>У вас имеется запись на {date} в {time}.</p>
                            <p>На {medicalServiceName}.</p>
                            <p>К {doctorFullName}.</p>
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
        /// Generates an email confirmation token for the specified account.
        /// </summary>
        /// <param name="account">The account model for which to generate the token.</param>
        /// <returns>A protected token as a string.</returns>
        private string GenerateEmailConfirmationToken(string email)
        {
            var protectedToken = _dataProtector.Protect(email);
            return protectedToken;
        }

        /// <summary>
        /// Asynchronously sends an email message.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="message">The HTML message body of the email.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        private async Task SendEmailAsync(string email, string subject, string message)
        {
            // Create, configure and send email message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Inno Clinic", _email));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_email, _emailAppPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}