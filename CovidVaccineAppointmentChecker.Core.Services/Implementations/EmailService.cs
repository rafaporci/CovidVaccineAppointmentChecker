using CovidVaccineAppointmentChecker.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mail;

namespace CovidVaccineAppointmentChecker.Core.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly ILogger _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(SmtpClient smtpClient, ILogger<EmailService> logger, EmailSettings emailSettings)
        {
            this._smtpClient = smtpClient;
            this._logger = logger;
            this._emailSettings = emailSettings;
        }

        public bool Send(string subject, string body)
        {
            var message = new MailMessage(new MailAddress(_emailSettings.From), new MailAddress(_emailSettings.To))
            {
                IsBodyHtml = true,
                Subject = subject,
                Body = body
            };

            try
            {
                this._smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error sending e-mail", new object[0]);
            }

            return false;
        }
    }
}
