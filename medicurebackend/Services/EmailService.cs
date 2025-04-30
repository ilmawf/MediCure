using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace medicurebackend.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;

        // Constructor to inject configuration values for email
        public EmailService(IConfiguration configuration)
        {
            // Get SMTP settings from appsettings.json and ensure they're not null
            _smtpServer = configuration["Email:SmtpServer"] ?? throw new ArgumentNullException("SMTP Server is not configured.");
            _smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? "587");  // Defaulting to 587 if missing
            _smtpUser = configuration["Email:SmtpUser"] ?? throw new ArgumentNullException("SMTP User is not configured.");
            _smtpPassword = configuration["Email:SmtpPassword"] ?? throw new ArgumentNullException("SMTP Password is not configured.");
            _senderEmail = configuration["Email:SenderEmail"] ?? throw new ArgumentNullException("Sender Email is not configured.");
        }

        // Method to send email
        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            // Create the email message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("MediCure Health", _senderEmail));
            emailMessage.To.Add(new MailboxAddress("", recipientEmail));
            emailMessage.Subject = subject;

            // Create the email body (you can use TextBody or HtmlBody)
            var bodyBuilder = new BodyBuilder { TextBody = body };  // Text email
            emailMessage.Body = bodyBuilder.ToMessageBody();

            // Send the email using SMTP
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpServer, _smtpPort, false);  // false = don't use SSL
                await client.AuthenticateAsync(_smtpUser, _smtpPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
