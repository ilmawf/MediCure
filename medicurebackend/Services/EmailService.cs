using SendinBlue.Client;
using SendinBlue.Client.Api;
using SendinBlue.Client.Model;
using System.Threading.Tasks;

namespace medicurebackend.Services
{
    public class EmailService
    {
        private readonly string _brevoApiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(string brevoApiKey, string fromEmail, string fromName)
        {
            _brevoApiKey = brevoApiKey;
            _fromEmail = fromEmail;
            _fromName = fromName;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var apiInstance = new TransactionalEmailsApi();
            var sendSmtpEmail = new SendSmtpEmail(
                sender: new SendSmtpEmailSender(_fromEmail, _fromName),
                to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(toEmail) },
                subject: subject,
                htmlContent: message
            );

            try
            {
                var response = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
                if (response != null)
                {
                    Console.WriteLine("Email sent successfully: " + response.MessageId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending email via Brevo: " + ex.Message);
            }
        }
    }
}
