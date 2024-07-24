using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace ModuleImplementation.Services
{
    public class EmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IConfiguration _configuration;

        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task SendEmailNotification(string recipientEmail, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["SmtpSettings:SmtpServer"];
                var smtpPort = int.Parse(_configuration["SmtpSettings:SmtpPort"]);
                var smtpUsername = _configuration["SmtpSettings:SmtpUsername"];
                var smtpPassword = _configuration["SmtpSettings:SmtpPassword"];

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(smtpUsername);
                    mailMessage.To.Add(recipientEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    await client.SendMailAsync(mailMessage);

                    _logger.LogInformation($"Email sent successfully to {recipientEmail}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error sending email.");

                // Optionally rethrow the exception for higher-level error handling
                throw;
            }
        }

    }
}
