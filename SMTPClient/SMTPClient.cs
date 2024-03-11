using Email.BL;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace SMTPClient
{
    public class SMTPClient
    {
        private SmtpClient _smtpClient;
       

        public SMTPClient(string address = "smtp.gmail.com", int port = 587, string login = null, string password = null)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException(nameof(address));

            if(string.IsNullOrWhiteSpace(login)) throw new ArgumentNullException(nameof(login));

            if(string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            _smtpClient = new SmtpClient(address,port);
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new NetworkCredential(login, password);
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.UseDefaultCredentials = false;
        }

        public async Task SendEmail(IMessage message, string file = null, string displayName = null)
        {
            try
            {
                MailAddress from = new MailAddress(message.From, displayName);
                MailAddress to = new MailAddress(message.To);
                MailMessage m = new MailMessage(from, to);
                m.Subject = message.Subject;
                m.Body = message.Text;

                if (file != null && File.Exists(file))
                {
                    m.Attachments.Add(new Attachment(file));
                }

                await _smtpClient.SendMailAsync(m);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            finally
            {

                _smtpClient.Dispose();
            }
        }



    }
}
