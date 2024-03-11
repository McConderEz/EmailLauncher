using MailKit.Net.Pop3;
using MimeKit;

namespace POP3Client
{
    public class POP3Client
    {
        private Pop3Client _pop3Client;
        public POP3Client(string address = "pop3.gmail.com", int port = 993, string login = null, string password = null)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentNullException(nameof(address));

            if (string.IsNullOrWhiteSpace(login)) throw new ArgumentNullException(nameof(login));

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            _pop3Client = new Pop3Client();
            _pop3Client.Connect(address, port, true);
            _pop3Client.Authenticate(login, password);

        }


        public void ReadMessages(int c = 10)
        {

            int messageCount = _pop3Client.GetMessageCount();
            int count = messageCount > c ? c : messageCount;

            for (int i = messageCount - count; i < messageCount; i++)
            {
                var message = _pop3Client.GetMessage(i);
                Console.WriteLine($"ID: {i}\nSubject: {message.Subject}\nFrom: {message.From}\nTo: {message.To}");
                Console.WriteLine(new string('-', 20));
            }

        }

        public void ReadMessageById(int id)
        {
            int messageCount = _pop3Client.GetMessageCount();

            if (id >= 0 && id < messageCount)
            {
                var message = _pop3Client.GetMessage(id);
                Console.WriteLine("Subject: " + message.Subject);
                Console.WriteLine("From: " + message.From);
                Console.WriteLine("To: " + message.To);
                Console.WriteLine("Body: " + message.TextBody);

                foreach (var attachment in message.Attachments)
                {
                    var fileName = attachment.ContentDisposition?.FileName ?? attachment.ContentType.Name;
                    using (var stream = System.IO.File.Create(Environment.CurrentDirectory + "\\" + fileName))
                    {
                        if (attachment is MimePart)
                        {
                            var part = (MimePart)attachment;
                            part.Content.DecodeTo(stream);
                        }
                    }
                    Console.WriteLine("Прикрепленные: " + fileName);
                }

            }
        }
    }
}
